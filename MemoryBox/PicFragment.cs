

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MemoryBox
{
    class PicFragment : DialogFragment
    {
        private ImageView picture;
        private ImageView inputPicture;
        private TextView text;
        private string inputText;
        private Context context;
        
        private bool isNew = false;

        public PicFragment(ImageView infPicture, string infText)
        {
            inputPicture = infPicture;
            inputText = infText;
            isNew = true;
        }

        public PicFragment()
        { }
        
        public PicFragment(ButtonPicMemories button)
        {
            inputPicture = button.Picture;
            inputText = button.TextMemory;
            isNew = true;
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            context = activity.Application.BaseContext;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            
            var view = inflater.Inflate(Resource.Layout.CallPic, container, false);

            picture = view.FindViewById<ImageView>(Resource.Id.viewPic);
            text = view.FindViewById <TextView>(Resource.Id.callPicText);
            Dialog.SetCancelable(true);
            Dialog.SetCanceledOnTouchOutside(true);

            if (isNew)
            {
                setPic();
            }
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }

        public void setPic()
        {
            text.SetText(inputText, TextView.BufferType.Normal);
            BitmapDrawable drawable = new BitmapDrawable();
            drawable = (BitmapDrawable)inputPicture.Drawable;
            if (drawable != null)
            { 
            Bitmap bitmap = drawable.Bitmap;
            picture.SetImageBitmap(bitmap);
                bitmap.Dispose();
            }
            
        }

    }
}