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

namespace MemoryBox
{
    public class SubmitTextEventArgs : EventArgs
    {
        private string title;
        private string text;
        private string created;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string Created
        {
            get { return created; }
            set { created = value; }
        }

        public SubmitTextEventArgs(string infTitle, string infText) : base()
        {
            Title = infTitle;
            Text = infText;
            created = Profile.CurrentProfile.Name;
        }


    }

    

    class CreateTextFragment : DialogFragment
    {
        private EditText inputTitle;
        private EditText inputText;
        private Button createMem;
        private View parent;

        public event EventHandler<SubmitTextEventArgs> onSubmitText;

        


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.CreateTextMemory, container, false);
            parent = view;
            inputTitle = view.FindViewById<EditText>(Resource.Id.enterTextTitle);
            inputText = view.FindViewById<EditText>(Resource.Id.enterTextText);
            createMem = view.FindViewById<Button>(Resource.Id.submitTextMemory);

            createMem.Click += delegate
            {
                onSubmitText.Invoke(this, new SubmitTextEventArgs(inputTitle.Text, inputText.Text));
                this.Dismiss();
            };

            return view;
        }


        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.Animation_AppCompat_DropDownUp;

            base.OnActivityCreated(savedInstanceState);
        }
    }
}