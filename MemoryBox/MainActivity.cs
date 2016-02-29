using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;
using System;

namespace MemoryBox
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {

        private RelativeLayout cover;
        private ToggleButton toggleMusic;
        private MediaPlayer player;
        private Button infoButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.Hide();
            SetContentView(Resource.Layout.Main);
            cover = FindViewById<RelativeLayout>(Resource.Id.titleScreen);
            infoButton = FindViewById<Button>(Resource.Id.infoButton);
            player = MediaPlayer.Create(this, Resource.Raw.avril_14th);
            toggleMusic = FindViewById<ToggleButton>(Resource.Id.toggleMusic);
            player.Start();
            player.Looping = true;

            infoButton.Click += (object sender, EventArgs args) =>
            {
                InfoFragment dialogInfo = new InfoFragment();
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                dialogInfo.Show(transaction,"info fragment");
            };

            cover.Click += delegate
            {
                StartActivity(typeof(Login));
            };


            toggleMusic.Click += (o, s) =>

            {
                if (toggleMusic.Checked)
                {
                    player.Start();
                    toggleMusic.SetBackgroundResource(Android.Resource.Drawable.IcMediaPause);

                }
                else
                {
                    toggleMusic.SetBackgroundResource(Android.Resource.Drawable.IcMediaPlay);
                    player.Pause();
                }
            };


        }

        private void ToggleMusic_Click(object sender, System.EventArgs e)
        {
            if (toggleMusic.Checked)
            {
                player.Release();
                player = MediaPlayer.Create(this, Resource.Raw.avril_14th);
                player.Start();
                player.Looping = true;
                toggleMusic.SetBackgroundResource(Android.Resource.Drawable.IcMediaPause);

            }
            else
                toggleMusic.SetBackgroundResource(Android.Resource.Drawable.IcMediaPlay);
                player.Stop();
        }

        public override void OnBackPressed()
        {
            Finish();
            Process.KillProcess(Process.MyPid());
        }





    }
}

