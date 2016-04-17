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
using Xamarin.Facebook.Login.Widget;

namespace MemoryBox
{
    public class CreateMemBoxFragment : DialogFragment
    {

        private EditText memBoxName;
        private Button createMemBox;
        private ProfilePictureView profilePic;
        private TextView loggedInAs;
        


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public EventHandler<CreateNewMemBoxArgs> CreateMemBox;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            var view = inflater.Inflate(Resource.Layout.createMemBox, container, false);

            memBoxName = view.FindViewById<EditText>(Resource.Id.inputMemBoxName);
            createMemBox = view.FindViewById<Button>(Resource.Id.submitNewMemBox);
            profilePic = view.FindViewById<ProfilePictureView>(Resource.Id.profilepic1);
            profilePic.ProfileId = Profile.CurrentProfile.Id;
            loggedInAs = view.FindViewById<TextView>(Resource.Id.loggedInAs);
            loggedInAs.SetText("Logged In As: "+Profile.CurrentProfile.Name, TextView.BufferType.Normal);
            

            createMemBox.Click += delegate
            {
                CreateMemBox.Invoke(this, new CreateNewMemBoxArgs(memBoxName.Text));
                
            };

            return view;


        }

    }


    public class CreateNewMemBoxArgs : EventArgs
    {
        private String text;

        public String Text
        {
            get { return text;  }
            set { text = value; }
        }


        public CreateNewMemBoxArgs(String inf_text) : base()
        {
            text = inf_text;
        }


    }
}