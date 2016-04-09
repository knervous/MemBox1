using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;
using System;
using Microsoft.WindowsAzure.MobileServices;
using Android.Content.PM;
using Java.Security;
using Xamarin.Facebook.Login;
using Xamarin.Facebook;
using Android.Support.V4.App;
using Xamarin.Facebook.Login.Widget;
using Java.Lang;
using Android.Content;
using Android.Runtime;

namespace MemoryBox
{
	[Activity (MainLauncher = true)]
	public class MainActivity : Activity, IDisposable, IFacebookCallback


	{

		private RelativeLayout cover;

		private ToggleButton toggleMusic;

		private MediaPlayer player;

		private Button infoButton;

        LoginResult loginResult;

        const string applicationURL = @"https://testapppaul.azurewebsites.net";
        const string localDbFilename = "localstore.db";

        private ICallbackManager callBackManager;

        public static MobileServiceClient client = new MobileServiceClient(applicationURL);      



        protected override void OnCreate (Bundle bundle)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
            FacebookSdk.SdkInitialize(this.ApplicationContext);
            CurrentPlatform.Init();
            ActionBar.Hide ();
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
			cover =  FindViewById<RelativeLayout> (Resource.Id.titleScreen);
			infoButton = FindViewById<Button> (Resource.Id.infoButton);
			player = MediaPlayer.Create (this, Resource.Raw.avril_14th);
			toggleMusic = FindViewById<ToggleButton> (Resource.Id.toggleMusic);
			player.Start ();
			player.Looping = true;
            
            LoginButton button = FindViewById<LoginButton>(Resource.Id.login_button);

            button.SetReadPermissions("user_friends");

            callBackManager = CallbackManagerFactory.Create();

            button.RegisterCallback(callBackManager, this);
            
			//cover.Click += delegate {
                
   //             StartActivity (typeof(Login));
   //             Finish();
			//};
			toggleMusic.Click += (o, s) => {


                Console.WriteLine("USER ID: " + loginResult.AccessToken.UserId);
                if (toggleMusic.Checked) {
					player.Start ();
					toggleMusic.SetBackgroundResource (Android.Resource.Drawable.IcMediaPause);
				} else {
					toggleMusic.SetBackgroundResource (Android.Resource.Drawable.IcMediaPlay);
					player.Pause ();
				}
			};


		}


        public override void OnBackPressed ()
		{
			Finish ();
			Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
            
		}

        public void OnCancel()
        {
           // Toast.MakeText(this.ApplicationContext, "You need to sign in to continue", 5);
        }

        public void OnError(FacebookException p0)
        {
            //throw new NotImplementedException();
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            loginResult = result as LoginResult;
            Console.WriteLine("USER ID: "+loginResult.AccessToken.UserId);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            callBackManager.OnActivityResult(requestCode, (int)requestCode, data);
            
        }
    }
    
}
