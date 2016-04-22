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


                Bitmap bitmap = BitmapFactory.DecodeStream(rs.OpenInputStream(data.Data));
                byte[] bitmapData;
                using (var stream = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    bitmapData = stream.ToArray();
                }

                /*
                   UPLOADING TO IMGUR

                */

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


                onSubmitPic.Invoke(this, new SubmitPicEventArgs(link, picText.Text));
                Dismiss();

            }

        }

        



        
    }



}