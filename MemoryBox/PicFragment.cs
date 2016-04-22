

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Square.Picasso;


namespace MemoryBox
{
    class PicFragment : DialogFragment
    {


        private ImageView picture;
        private TextView text;
        private TextView link;
        private TextView createdBy;
        private ScrollView scrollView;
        private string title;
        private string url;
        private string created;
        private Context context;

        public PicFragment(string infTitle, string infUrl, string infCreated)
        {
            title = infTitle;
            url = infUrl;
            created = infCreated;
        }

        public PicFragment()
        { }
        

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            context = activity.Application.BaseContext;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            
            var view = inflater.Inflate(Resource.Layout.CallPic, container, false);
            scrollView = view.FindViewById<ScrollView>(Resource.Id.picZoom);
            picture = view.FindViewById<ImageView>(Resource.Id.viewPic);
            text = view.FindViewById <TextView>(Resource.Id.callPicText);
            link = view.FindViewById<TextView>(Resource.Id.callPicURL);
            createdBy = view.FindViewById<TextView>(Resource.Id.callPicCreated);
            Dialog.SetCancelable(true);
            Dialog.SetCanceledOnTouchOutside(true);

            setPic(view);
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }


        public void setPic(View view)
        {
            text.SetText(title, TextView.BufferType.Normal);
            link.SetText("Link: "+url, TextView.BufferType.Normal);
            createdBy.SetText("Created By: "+created, TextView.BufferType.Normal);
            Picasso.With(view.Context)
                .Load(url)
                .Into(picture);
        }

    }

   
    
}