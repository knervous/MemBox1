using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Facebook;
using Java.Lang;
using System.Threading;
using System.Threading.Tasks;


namespace MemoryBox
{

    public class SubmitPicEventArgs : EventArgs
    {
        private string url;
        private string pictureText;
        private string pictureCreated;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string PictureText
        {
            get { return pictureText; }
            set { pictureText = value; }
        }

        public string PictureCreated
        {
            get { return pictureCreated; }
            set { pictureCreated = value; }
        }


        public SubmitPicEventArgs(string infUrl, string infText) : base()
        {
            url = infUrl;
            pictureText = infText;
            pictureCreated = Profile.CurrentProfile.Name;
        }


    }


    class CreatePicFragment : DialogFragment
    {


        private EditText picText;
        private string link;
        private View mView;
        private Button goToGallery;
        private Context context;
        private ContentResolver rs;
        private ProgressBar progressBar;
        private RelativeLayout background;

        public event EventHandler<SubmitPicEventArgs> onSubmitPic;



        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            context = activity.Application.BaseContext;
        }




        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            mView = inflater.Inflate(Resource.Layout.CreatePicMemory, container, false);
            picText = mView.FindViewById<EditText>(Resource.Id.enterPicText);
            goToGallery = mView.FindViewById<Button>(Resource.Id.enterPicButton);
            progressBar = mView.FindViewById<ProgressBar>(Resource.Id.progressBar1);
            background = mView.FindViewById<RelativeLayout>(Resource.Id.createPicLayout);

            progressBar.Visibility = ViewStates.Invisible;
            rs = context.ContentResolver;
            goToGallery.Click += delegate (object sender, EventArgs e)
            {
                Intent intent = new Intent();
                intent.SetType("image/*");
                intent.SetAction(Intent.ActionGetContent);
                this.StartActivityForResult(Intent.CreateChooser(intent, "Select a Photo"), 0);
            };

            return mView;
        }



        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.Animation_AppCompat_DropDownUp;
            base.OnActivityCreated(savedInstanceState);
        }


        public async override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {


                /*
                   UPLOADING TO IMGUR

                */
                
                background.RemoveAllViews();
                background.AddView(progressBar);
                progressBar.Visibility = ViewStates.Visible;

                Toast.MakeText(Activity, "Loading Image", ToastLength.Short).Show();
                var mResult = await LoadImageAsync(data);
                Toast.MakeText(Activity, "Uploading Image", ToastLength.Long).Show();
                
                await UploadToImgurAsync(mResult);
                
                onSubmitPic.Invoke(this, new SubmitPicEventArgs(link, picText.Text));
                
                Dismiss();

                



            }

        }


        private Task<byte[]> LoadImageAsync(Intent data)
        {
            return Task.Factory.StartNew(() => LoadImage(data));
        }

        private byte[] LoadImage(Intent data)
        {
            Bitmap bitmap = BitmapFactory.DecodeStream(rs.OpenInputStream(data.Data));
            byte[] bitmapData;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }

        private Task UploadToImgurAsync(byte[] bitmapData)
        {
            return Task.Factory.StartNew(() => UploadToImgur(bitmapData));
        }

        private void UploadToImgur(byte[] bitmapData)
        {
            using (var w = new WebClient())
            {
                var values = new NameValueCollection
                    {
                        { "image", Convert.ToBase64String(bitmapData) }
                    };
                w.Headers.Add("Authorization", "Client-ID 14bb8bcd2a1d1f8");
                byte[] response = w.UploadValues("https://api.imgur.com/3/image", "POST", values);
                string responsebody = Encoding.UTF8.GetString(response);

                Org.Json.JSONObject root = new Org.Json.JSONObject(responsebody);
                Org.Json.JSONObject uploadData = root.GetJSONObject("data");
                link = uploadData.Get("link").ToString();

            }


            
        }
    }



}
