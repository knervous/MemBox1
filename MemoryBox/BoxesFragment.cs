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
    
    public class BoxesFragment : Android.Support.V4.App.Fragment
    {
        private Button createMem;
        private List<MemoryModel> boxes;
        private MemoryModel currentModel;
        private ListView boxListView;
        private CreateMemBoxFragment createMemFragment;
        private ArrayAdapter<string> listAdapter;
        private MemoryListViewAdapter memoryListViewAdapter;
        public event EventHandler<CreateMemoryBoxEventArgs> createMemoryBox;
        public event EventHandler<EnterMemoryBoxEventArgs> enterMemoryBox;
        public event EventHandler<EventArgs> deleteMemoryBox;

        

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Boxes, container, false);
            createMem = view.FindViewById<Button>(Resource.Id.toMemories);
            createMem.Click += delegate
            {
                createMemoryBox.Invoke(this, new CreateMemoryBoxEventArgs());
                
            };

            boxListView = view.FindViewById<ListView>(Resource.Id.memListView);



            memoryListViewAdapter = new MemoryListViewAdapter(view.Context);
            boxListView.Adapter = memoryListViewAdapter;

            boxListView.ItemClick += BoxListView_ItemClick;
            boxListView.ItemLongClick += BoxListView_ItemLongClick;


            return view;
        }

        private void BoxListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            currentModel = memoryListViewAdapter.Memories[e.Position];
            deleteMemoryBox.Invoke(this, new EventArgs());
        }

        private void BoxListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            currentModel = memoryListViewAdapter.Memories[e.Position];
            enterMemoryBox.Invoke(this, new EnterMemoryBoxEventArgs());
        }


        public MemoryModel CurrentBox
        {
            get { return currentModel; }
            set { currentModel = value; }
        }

        public MemoryListViewAdapter MemoryListViewAdapter
        {
            get { return memoryListViewAdapter; }
            set { memoryListViewAdapter = value; }
        }

        public override void OnPause()
        {
            
            base.OnPause();

        }

        public override void OnResume()
        {
            base.OnResume();
        }

        public override void OnDetach()
        {
     
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