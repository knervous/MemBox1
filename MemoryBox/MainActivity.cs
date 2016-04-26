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
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Newtonsoft.Json.Linq;
using Android.Content.PM;
using Android.Widget;

namespace MemoryBox
{



    [Activity(MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FragmentActivity, IDisposable, IFacebookCallback


    {
        private BoxesFragment boxFragment;
        private HomeScreenFragment homeScreenFragment;
        private SupportFragment currentFragment;
        private MemoriesFragment mMemoriesFragment;
        private CreateMemBoxFragment createMemBoxFragment;
        private Stack<SupportFragment> stack;
        const string applicationURL = @"https://paulmemorybox.azurewebsites.net";
        const string localDbFilename = "localstore1.db";
        private ICallbackManager callBackManager;
        public static MobileServiceClient client;
        private MemoryModel memoryModelTemp;
        private IMobileServiceSyncTable<SerializedMemory> syncMemModel;
        private IMobileServiceTable<SerializedMemory> table;
        

        private async Task InitLocalStoreAsync()
        {
            string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), localDbFilename);
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path).Dispose();
            }

            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<SerializedMemory>();

            
            await client.SyncContext.InitializeAsync(store);

        }

        private async void OnRefreshItemsSelected()
        {
            await SyncAsync(pullData: true);
            await RefreshItemsFromTableAsync();
        }

        private async Task RefreshItemsFromTableAsync()
        {
            try
            {
                var list = await syncMemModel.ToListAsync();
                boxFragment.MemoryListViewAdapter.Clear();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                foreach (SerializedMemory current in list)
                {
                    var temp = JsonConvert.DeserializeObject<MemoryModel>(current.MemoryBox, settings);
                    temp.Id = current.Id;
                    boxFragment.MemoryListViewAdapter.Add(temp);
                }
            }
            catch (Exception dungle) { Console.WriteLine(dungle.StackTrace); }

        }

        private async Task SyncAsync(bool pullData = false)
        {
            try
            {
                await client.SyncContext.PushAsync();

                if (pullData) { await syncMemModel.PullAsync("allitems", syncMemModel.CreateQuery()); }
            }
            catch (Exception e) { Console.WriteLine(e.StackTrace); }
        }



        protected override async void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Xamarin.Insights.Initialize(XamarinInsights.ApiKey, this);
            FacebookSdk.SdkInitialize(this.ApplicationContext);
            CurrentPlatform.Init();
            client = new MobileServiceClient(applicationURL);
            table = client.GetTable<SerializedMemory>();

            ActionBar.Hide();

            homeScreenFragment = new HomeScreenFragment();
            boxFragment = new BoxesFragment();
            mMemoriesFragment = new MemoriesFragment();
            currentFragment = homeScreenFragment;
            createMemBoxFragment = new CreateMemBoxFragment();
            stack = new Stack<SupportFragment>();
            syncMemModel = client.GetSyncTable<SerializedMemory>();

            OnRefreshItemsSelected();

            homeScreenFragment.facebookLogin += async delegate
            {

                if (isLoggedIn())
                {
                    Toast.MakeText(this, "Logging in via Facebook...", ToastLength.Short).Show();
                    await InitLocalStoreAsync();
                    await ShowFragment(boxFragment);


                }
                else
                    LoginManager.Instance.LogInWithReadPermissions(this, new List<string> { "public_profile", "user_friends", "email" });
            };

            boxFragment.createMemoryBox += delegate
            {
                
                
                Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction();
                createMemBoxFragment = new CreateMemBoxFragment();
                createMemBoxFragment.Show(transaction, "signup fragment");
                createMemBoxFragment.CreateMemBox += async delegate (object sender, CreateNewMemBoxArgs args)

                {
                    var name = args.Text + " - " + Profile.CurrentProfile.Name;
                    var temp = new MemoryModel() { Name = name, Owner = Profile.CurrentProfile.Name };
                    var serialized = JsonConvert.SerializeObject(temp);
                    SerializedMemory mem = new SerializedMemory() { MemoryBox = serialized };
                    Toast.MakeText(this, "Creating MemoryBox! :)", ToastLength.Long).Show();
                    await syncMemModel.InsertAsync(mem);
                    await SyncAsync();
                    createMemBoxFragment.Dismiss();
                    OnRefreshItemsSelected();
                };


            };

            boxFragment.refreshMemoryBox += delegate
            {
                OnRefreshItemsSelected();
                Toast.MakeText(this, "Syncing Local and Server Databases, Please Wait!", ToastLength.Long).Show();
            };

            boxFragment.enterMemoryBox += async delegate (object sender, EnterMemoryBoxEventArgs args)
            {
                var serialized = JsonConvert.SerializeObject(boxFragment.CurrentBox);
                memoryModelTemp = JsonConvert.DeserializeObject<MemoryModel>(serialized);
                mMemoriesFragment = new MemoriesFragment(boxFragment.CurrentBox);
                await ShowFragment(mMemoriesFragment);
            };

            boxFragment.deleteMemoryBox += async delegate (object sender, EventArgs args)
            {
                if(Profile.CurrentProfile.Name == boxFragment.CurrentBox.Owner || boxFragment.CurrentBox.Owner == "Paul Johnson")
                {
                    var serialized = JsonConvert.SerializeObject(boxFragment.CurrentBox);
                    var mem = new SerializedMemory() { MemoryBox = serialized, Id = boxFragment.CurrentBox.Id };
                    Toast.MakeText(this, "Deleting MemoryBox! :(", ToastLength.Short).Show();
                    await syncMemModel.DeleteAsync(mem);
                    await SyncAsync();
                    await RefreshItemsFromTableAsync();
                    OnRefreshItemsSelected();
                }
                else
                {
                    Toast.MakeText(this, "Only The Owner Can Delete A MemoryBox!", ToastLength.Short).Show();
                }
                
            };




            callBackManager = CallbackManagerFactory.Create();
            await ShowFragment(currentFragment);
            }




        public async override void OnBackPressed()
        {
            if (currentFragment == homeScreenFragment)
            {
                Finish();
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }

            if (currentFragment == mMemoriesFragment)
            {
                var serialized = JsonConvert.SerializeObject(mMemoriesFragment.CurrentMemory);
                SerializedMemory memToDelete = new SerializedMemory() { MemoryBox = serialized, Id = memoryModelTemp.Id };
                SerializedMemory mem = new SerializedMemory() { MemoryBox = serialized, Id = memoryModelTemp.Id };
                Toast.MakeText(this, "Saving MemoryBox...", ToastLength.Short).Show();
                await syncMemModel.UpdateAsync(mem);
                OnRefreshItemsSelected();
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
            //ShowFragment(boxFragment);
            //Console.WriteLine("SUCCESS OPENING NEW");
        }

        private async Task ShowFragment(SupportFragment fragment)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            if (!(fragment is HomeScreenFragment))
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
