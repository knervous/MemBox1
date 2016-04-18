using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook;
using Android.Support.V4.App;

namespace MemoryBox
{
    [Activity()]
    public class BoxesFragment : Android.Support.V4.App.Fragment
    {
        public Button createMem;
        public List<string> boxes;
        public ListView boxListView;
        public CreateMemBoxFragment createMemFragment;
        public ArrayAdapter<string> listAdapter;
        public MyProfileTracker profileTracker;
        public event EventHandler<CreateMemoryBoxEventArgs> createMemoryBox;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Boxes, container, false);
            createMem = view.FindViewById<Button>(Resource.Id.toMemories);
            profileTracker = new MyProfileTracker();
            profileTracker.mOnProfileChanged += delegate (object sender, OnProfileChangedEventArgs e)
            {
 
            };
            profileTracker.StartTracking();



            createMem.Click += delegate
            {
                createMemoryBox.Invoke(this, new CreateMemoryBoxEventArgs());

                //Android.App.FragmentManager fm = Activity.FragmentManager;
                //createMemFragment = new CreateMemBoxFragment();
                //createMemFragment.Show(fm, "signup fragment");
                //createMemFragment.CreateMemBox += delegate (object sender, CreateNewMemBoxArgs args) 
                
                //{
                //    var name = args.Text;
                //};
                
            };

            boxes = new List<string>();
            boxListView = view.FindViewById<ListView>(Resource.Id.memListView);

            boxes.Add("Box 1");
            boxes.Add("Box 2");
            boxes.Add("Box 3");

            listAdapter = new ArrayAdapter<string>(view.Context, Android.Resource.Layout.SimpleListItem1, boxes);
            boxListView.Adapter = listAdapter;

            return view;
        }

        public override void OnPause()
        {
            profileTracker.StopTracking();
            base.OnPause();

        }

        public override void OnResume()
        {
            base.OnResume();
        }

        public override void OnDetach()
        {
            profileTracker.StopTracking();
            base.OnDetach();
        }


    }

    public class CreateMemoryBoxEventArgs : EventArgs
    {

        public CreateMemoryBoxEventArgs() : base()
        {
        }

    }

    public class BoxList : ListActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.Hide();

        }
    }
}