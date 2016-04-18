using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook;
using Android.Media;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Facebook.Login;
using Android.Support.V4.App;

namespace MemoryBox
{
    public class HomeScreenFragment : Android.Support.V4.App.Fragment, IDisposable
    {

        private ToggleButton toggleMusic;
        private MediaPlayer player;
        private Button infoButton;
        private RelativeLayout cover;
        public event EventHandler<LoginEventArgs> facebookLogin;
        private RelativeLayout Cover
        {
            get { return cover; }
            set { cover = value; }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomeScreen, container, false);
            infoButton = view.FindViewById<Button>(Resource.Id.infoButton);
            player = MediaPlayer.Create(view.Context, Resource.Raw.avril_14th);
            toggleMusic = view.FindViewById<ToggleButton>(Resource.Id.toggleMusic);
            cover = view.FindViewById<RelativeLayout>(Resource.Id.titleScreen);
            
            Xamarin.Insights.Initialize(XamarinInsights.ApiKey, view.Context);
            CurrentPlatform.Init();

            player.Start();
            player.Looping = true;


            cover.Click += delegate
            {
                facebookLogin.Invoke(this, new LoginEventArgs());
            };

            toggleMusic.Click += (o, s) => {

                if (toggleMusic.Checked)
                {
                    player.Start();
                    toggleMusic.SetBackgroundResource(Android.Resource.Drawable.IcMediaPause);
                }
                else {
                    toggleMusic.SetBackgroundResource(Android.Resource.Drawable.IcMediaPlay);
                    player.Pause();
                }
            };

            return view;
        }


        public override void OnPause()
        {
            base.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        




    }
}