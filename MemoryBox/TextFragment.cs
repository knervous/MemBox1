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
    class TextFragment : DialogFragment
    {
        private TextView title;
        private TextView text;
        private string inputTitle;
        private string inputText;

        public TextFragment(string infTitle, string infText)
        {
            inputTitle = infTitle;
            inputText = infText;
        }

        public TextFragment()
        {

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.CallText, container, false);

            title = view.FindViewById<TextView>(Resource.Id.callTextTitle);
            text = view.FindViewById<TextView>(Resource.Id.callText);

            setText();

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }

        public void setText()
        {
            title.SetText(inputTitle, TextView.BufferType.Normal);
            text.SetText(inputText, TextView.BufferType.Normal);
        }
    }
}