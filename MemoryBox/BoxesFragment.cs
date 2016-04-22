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
        public List<MemoryModel> boxes;
        public MemoryModel currentModel;
        public ListView boxListView;
        public CreateMemBoxFragment createMemFragment;
        public ArrayAdapter<string> listAdapter;
        public MyProfileTracker profileTracker;
        private MemoryListViewAdapter memoryListViewAdapter;
        public event EventHandler<CreateMemoryBoxEventArgs> createMemoryBox;
        public event EventHandler<EnterMemoryBoxEventArgs> enterMemoryBox;


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
                
            };


            boxes = new List<MemoryModel>();
            boxListView = view.FindViewById<ListView>(Resource.Id.memListView);

            boxes.Add(new MemoryModel());
            boxes.Add(new MemoryModel());
            boxes.Add(new MemoryModel());
            boxes[0].Name = "Mem Box 1";
            boxes[1].Name = "Mem Box 2";
            boxes[2].Name = "Mem Box 3";


            memoryListViewAdapter = new MemoryListViewAdapter(view.Context, boxes);
            boxListView.Adapter = memoryListViewAdapter;

            boxListView.ItemClick += BoxListView_ItemClick;

            return view;
        }

        private void BoxListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if(currentModel==null)
                currentModel = boxes[e.Position];
            
            enterMemoryBox.Invoke(this, new EnterMemoryBoxEventArgs());
        }

        public List<MemoryModel> Boxes
        {
            get { return boxes;  }
            set { boxes = value;  }
        }

        public MemoryModel CurrentBox
        {
            get { return currentModel; }
            set { currentModel = value; }
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
    public class EnterMemoryBoxEventArgs : EventArgs
    {
        public EnterMemoryBoxEventArgs() : base()
        {

        }

    }
    }