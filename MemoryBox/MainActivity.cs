using Android.App;
using Android.OS;
using System;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Facebook.Login;
using Xamarin.Facebook;
using Android.Support.V4.App;
using Android.Content;
using System.Collections.Generic;
using SupportFragment = Android.Support.V4.App.Fragment;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace MemoryBox
{



    [Activity(MainLauncher = true)]
    public class MainActivity : FragmentActivity, IDisposable, IFacebookCallback


    {
        private BoxesFragment boxFragment;
        private HomeScreenFragment homeScreenFragment;
        private SupportFragment currentFragment;
        private MemoriesFragment mMemoriesFragment;
        private CreateMemBoxFragment createMemBoxFragment;
        private Stack<SupportFragment> stack;
        const string applicationURL = @"https://testapppaul.azurewebsites.net";
        const string localDbFilename = "localstore.db";
        private ICallbackManager callBackManager;
        public static MobileServiceClient client = new MobileServiceClient(applicationURL);
        private MemoryModel memoryModel;
        private IMobileServiceSyncTable<MemoryModel> syncMemModel;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Insights.Initialize(XamarinInsights.ApiKey, this);
            FacebookSdk.SdkInitialize(this.ApplicationContext);
            CurrentPlatform.Init();
            ActionBar.Hide();
            SetContentView(Resource.Layout.Main);
            homeScreenFragment = new HomeScreenFragment();
            boxFragment = new BoxesFragment();
            mMemoriesFragment = new MemoriesFragment();
            currentFragment = homeScreenFragment;
            stack = new Stack<SupportFragment>();


            homeScreenFragment.facebookLogin += delegate
            {

                if (isLoggedIn())
                    ShowFragment(boxFragment);
                else
                    LoginManager.Instance.LogInWithReadPermissions(this, new List<string> { "public_profile", "user_friends", "email" });
            };

            boxFragment.createMemoryBox += delegate
            {
                Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction();
                createMemBoxFragment = new CreateMemBoxFragment();
                createMemBoxFragment.Show(transaction, "signup fragment");
                createMemBoxFragment.CreateMemBox += delegate (object sender, CreateNewMemBoxArgs args)

                {
                    var name = args.Text;
                    boxFragment.Boxes.Add(new MemoryModel() { Name = name });
                    //ShowFragment(mMemoriesFragment);
                    createMemBoxFragment.Dismiss();
                };


            };

            boxFragment.enterMemoryBox += delegate (object sender, EnterMemoryBoxEventArgs args)
            {
                var serialized = JsonConvert.SerializeObject(boxFragment.CurrentBox);
                var serial = new SerializedMemory() { Data = serialized };
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                boxFragment.CurrentBox = JsonConvert.DeserializeObject<MemoryModel>(serial.Data, settings);
                mMemoriesFragment = new MemoriesFragment(boxFragment.CurrentBox);
                ShowFragment(mMemoriesFragment);
            };




            callBackManager = CallbackManagerFactory.Create();
            ShowFragment(currentFragment);
        }


        

        public override void OnBackPressed()
        {
            if (currentFragment == homeScreenFragment)
            {
                Finish();
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }

            if (currentFragment == mMemoriesFragment)
            {
                var trans = SupportFragmentManager.BeginTransaction();
                trans.Detach(mMemoriesFragment);
                trans.Commit();
            }

            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                SupportFragmentManager.PopBackStack();
                currentFragment = stack.Pop();
            }
            else
            {
                Finish();
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }

        }


        public void OnCancel()
        {
            Console.WriteLine("cancelled");
        }

        public void OnError(FacebookException p0)
        {
            p0.PrintStackTrace();
            Console.WriteLine("FAILED");
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            LoginResult loginResult = result as LoginResult;
            ShowFragment(boxFragment);
            Console.WriteLine("SUCCESS OPENING NEW");
        }

        private void ShowFragment(SupportFragment fragment)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            if(!(fragment is HomeScreenFragment))
            { 
                trans.SetCustomAnimations(Resource.Animation.slide_in, Resource.Animation.slide_out);
            }
            trans.Add(Resource.Id.titleScreen1, fragment, "CurrentFragment");
            trans.Hide(currentFragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();
            stack.Push(currentFragment);
            currentFragment = fragment;
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            callBackManager.OnActivityResult(requestCode, (int)resultCode, data);

        }


        public bool isLoggedIn()
        {
            return (AccessToken.CurrentAccessToken != null && Profile.CurrentProfile != null);
        }

    }



    public class MyProfileTracker : ProfileTracker
    {

        public EventHandler<OnProfileChangedEventArgs> mOnProfileChanged;

        protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile)
        {
            if (mOnProfileChanged != null)
            {
                mOnProfileChanged.Invoke(this, new OnProfileChangedEventArgs(newProfile));
            }
        }
    }

    public class OnProfileChangedEventArgs : EventArgs
    {
        public Profile mProfile;

        public OnProfileChangedEventArgs(Profile profile)
        {
            mProfile = profile;
        }

    }


    public class LoginEventArgs : EventArgs
    {

        public LoginEventArgs() : base()
        {
        }

    }



}
