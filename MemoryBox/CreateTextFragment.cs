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

namespace MemoryBox
{
    public class SubmitTextEventArgs : EventArgs
    {
        private string title;
        private string text;

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

        public SubmitTextEventArgs(string infTitle, string infText) : base()
        {
            Title = infTitle;
            Text = infText;
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

            createMem.Click += CreateMem_Click;

            return view;
        }

        private void CreateMem_Click(object sender, EventArgs e)
        {
            onSubmitText.Invoke(this, new SubmitTextEventArgs(inputTitle.Text, inputText.Text));
            this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }
    }
}