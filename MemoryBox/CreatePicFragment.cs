using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.IO;
using Com.Nostra13.Universalimageloader.Core;
using Square.Picasso;
using Java.IO;


namespace MemoryBox
{

    public class SubmitPicEventArgs : EventArgs
    {
        private ImageView picture;
        private string pictureText;

        public ImageView Picture
        {
            get { return picture; }
            set { picture = value; }
        }

        public string PictureText
        {
            get { return pictureText; }
            set { pictureText = value; }
        }


        public SubmitPicEventArgs(ImageView infPicture, string infText) : base()
        {
            picture = infPicture;
            pictureText = infText;
        }


    }
    class CreatePicFragment : DialogFragment
    {


        private EditText picText;
        private Button goToGallery;
        private ImageView picture;
        private Context context;
        private ContentResolver rs;

        public event EventHandler<SubmitPicEventArgs> onSubmitPic;

        

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            context = activity.Application.BaseContext;
        }

        


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.CreatePicMemory, container, false);
            picText = view.FindViewById<EditText>(Resource.Id.enterPicText);
            goToGallery = view.FindViewById<Button>(Resource.Id.enterPicButton);
            picture = new ImageView(context);
            rs = context.ContentResolver;
            goToGallery.Click += delegate (object sender, EventArgs e)
            {
                Intent intent = new Intent();
                intent.SetType("image/*");
                intent.SetAction(Intent.ActionGetContent);
                this.StartActivityForResult(Intent.CreateChooser(intent, "Select a Photo"), 0);
            };

            return view;
        }

        private void GoToGallery_Click(object sender, EventArgs e)
        {

            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            this.StartActivityForResult(Intent.CreateChooser(intent, "Select a Photo"), 0);

        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }

        
        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) 
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if(resultCode == Result.Ok)
            {
                  Picasso.With(context)
                     .Load(data.Data)
                  .MemoryPolicy(MemoryPolicy.NoCache, MemoryPolicy.NoStore)

                   .Resize(300, 300)

                 .Into(picture);


                //  ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(context).Build();
                //  ImageLoader.Instance.Init(config);
                //ImageLoader.Instance.ClearMemoryCache();



                //Stream stream = rs.OpenInputStream(data.Data);
                //Bitmap bm = null;
                //Bitmap bitmap = BitmapFactory.DecodeStream(rs.OpenInputStream(data.Data));
                //MemoryStream fout = new MemoryStream();
                //bitmap.Compress(Bitmap.CompressFormat.Png, 100, fout);
                //bitmap = BitmapFactory.DecodeStream(fout);
                //bm = Bitmap.CreateScaledBitmap(bitmap, 300, 300, true);

                //picture.SetImageBitmap(BitmapFactory.DecodeStream(stream));
                //picture.SetImageBitmap(bitmap);
                //picture.SetImageURI(data.Data);
                //bm.Dispose();
                //bitmap.Dispose();


                Bitmap bm = DecodeBitmapFromStream(data.Data, 300, 300);

                ExportBitmapAsPNG(bm);

                bm.Dispose();



                onSubmitPic.Invoke(this, new SubmitPicEventArgs(picture, picText.Text));

                //Dispose();
                //GC.Collect();
                Dismiss();

            }

        }


        void ExportBitmapAsPNG(Bitmap bitmap)
        {




            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            
            var filePath = System.IO.Path.Combine(sdCardPath, "test.png");
            var stream = new FileStream(filePath, FileMode.Create);


            //MemoryStream stream = new MemoryStream();
            //bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
            //byte[] bitmapData = stream.ToArray();



            //System.IO.File.WriteAllBytes(mPath, bitmapData);

            //var fstream = new fileoutputstream(filepath, true);
            //using (var stream = new FileOutputStream(filePath, FileMode.Create))
            //{
            //    bitmap.compress(bitmap.compressformat.jpeg, 100, stream);
            //}

            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            bitmap.Dispose();
            stream.Flush();
            stream.Close();
        }

        private Bitmap DecodeBitmapFromStream (Android.Net.Uri data, int requestedWidth, int requestedHeight)
        {
            //decode with InJustDecodeBounds
            Stream stream = rs.OpenInputStream(data);
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeStream(stream);

            options.InSampleSize = CalculateInSampleSize(options, requestedWidth, requestedHeight);

            //decode the bitmap

            stream = rs.OpenInputStream(data);
            options.InJustDecodeBounds = false;
            Bitmap bitmap = BitmapFactory.DecodeStream(stream, null, options);

            return bitmap;

        }

        private int CalculateInSampleSize(BitmapFactory.Options options, int requestedWidth, int requestedHeight)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > requestedHeight || width > requestedWidth)
            {
                //image is bigger than desired

                int halfHeight = height / 2;
                int halfWidth = width / 2;

                while (halfHeight / inSampleSize > requestedHeight && halfWidth / inSampleSize > requestedWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return inSampleSize;

        }

        
    }
}