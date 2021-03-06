using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Xamarin.Facebook;
using Android.Views.Animations;

namespace MemoryBox
{

    public class MemoriesFragment : Android.Support.V4.App.Fragment, View.IOnTouchListener
    {

        
        private FrameLayout fl;
        private float _viewX;
        private float _viewY;
        private float initialX;
        private float initialY;
        private float endX;
        private float endY;
        private ImageView baseImage;
        private List<Button> voiceButtons;
        private List<Button> textButtons;
        private List<Button> picButtons;
        private List<FrameLayout.LayoutParams> prmList;
        private Button createText;
        private Button createPic;
        private Button createVoice;
        private PicFragment invokePic = new PicFragment();
        private View mView;
        private MemoryModel mMemoryModel;

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutFloat("viewX", _viewX);
        }

        public MemoriesFragment()
        {

        }
        public MemoriesFragment(MemoryModel memoryModel)
        {
            mMemoryModel = memoryModel;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Memories, container, false);
            mView = view;

            prmList = new List<FrameLayout.LayoutParams>();
            fl = view.FindViewById<FrameLayout>(Resource.Id.memFrame);
            Random rand = new Random();
            var random = rand.Next(1, 5);
            switch (random)
            {
                case 1:
                fl.SetBackgroundResource(Resource.Drawable.background_beach);
                break;
                case 2:
                    fl.SetBackgroundResource(Resource.Drawable.background_farm1);
                    break;
                case 3:
                    fl.SetBackgroundResource(Resource.Drawable.background_tree);
                    break;
                case 4:
                    fl.SetBackgroundResource(Resource.Drawable.background_winter1);
                    break;
                case 5:
                    // KEEP BG RESOURCE
                    break;

            }
            


            baseImage = (ImageView)view.FindViewById<ImageView>(Resource.Id.viewPic);
            

            voiceButtons = new List<Button>();
            textButtons = new List<Button>();
            picButtons = new List<Button>();

            createText = view.FindViewById<Button>(Resource.Id.CreateTextButton);
            createPic = view.FindViewById<Button>(Resource.Id.CreatePicButton);
            createVoice = view.FindViewById<Button>(Resource.Id.CreateVoiceButton);

            createText.Click += (sender, e) =>
            {
                CreateTextFragment dialogInfo = new CreateTextFragment();
                Android.App.FragmentManager transaction = Activity.FragmentManager;
                dialogInfo.Show(transaction, "signup fragment");
                dialogInfo.onSubmitText += CreateTextMemory;

            };

            createPic.Click += delegate (object sender, EventArgs e)
            {
                CreatePicFragment dialogInfo = new CreatePicFragment();
                Android.App.FragmentManager transaction = Activity.FragmentManager;
                dialogInfo.Show(transaction, "signup fragment");
                dialogInfo.onSubmitPic += CreatePicMemory;
            };

            createVoice.Click += (sender, e) =>
            {
                //CreateVoiceFragment dialogInfo = new CreateVoiceFragment();
                //Android.App.FragmentManager transaction = Activity.FragmentManager;
                //dialogInfo.Show(transaction, "signup fragment");
                Toast.MakeText(view.Context, "Coming soon!", ToastLength.Short).Show();
            };

            fl.Click += (sender, e) =>
            {
                Toast.MakeText(view.Context, "Double Click and Hold a Memory to View", ToastLength.Short).Show();
            };

            CreateButtons(mMemoryModel);

            return view;
        }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public MemoryModel CurrentMemory
        {
            get { return mMemoryModel; }
            set { mMemoryModel = value; }
        }



        private void CreateTextMemory(object sender, SubmitTextEventArgs e)
        {
            var prms = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.WrapContent);
            var dimensionRand = RandomNumber(100, 200);
            mMemoryModel.TextMemTitle.Add(e.Title);
            mMemoryModel.TextMemText.Add(e.Text);
            mMemoryModel.TextCreatedBy.Add(Profile.CurrentProfile.Name);
            textButtons.Add(new Button(mView.Context));
            textButtons[textButtons.Count-1].SetBackgroundResource(Resource.Drawable.textMem);
            prms.Height = dimensionRand;
            prms.Width = dimensionRand;
            textButtons[textButtons.Count - 1].SetOnTouchListener(this);
            fl.AddView(textButtons[textButtons.Count - 1], prms);
            textButtons[textButtons.Count - 1].LongClick += (sender1, e1) =>
            {
                if (Math.Abs(initialX - endX) < 50 && Math.Abs(initialX - endX) < 50)
                {
                    TextFragment dialogInfo = new TextFragment(e.Title, e.Text, e.Created);
                    Android.App.FragmentManager transaction = Activity.FragmentManager;
                    dialogInfo.Show(transaction, "signup fragment");
                }
            };
            Toast.MakeText(Activity, "Text Memory Created!", ToastLength.Short).Show();

        }

        private void CreatePicMemory(object sender, SubmitPicEventArgs e)
        {
            var prms = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.WrapContent);
            mMemoryModel.PicMemTitle.Add(e.PictureText);
            mMemoryModel.PicMemUrl.Add(e.Url);
            mMemoryModel.PicCreatedBy.Add(Profile.CurrentProfile.Name);
            picButtons.Add(new Button(mView.Context));
            picButtons[picButtons.Count - 1].SetBackgroundResource(Resource.Drawable.vidMem);
            picButtons[picButtons.Count - 1].SetOnTouchListener(this);

            var dimensionRand = RandomNumber(100, 200);
            prms.Height = dimensionRand;
            prms.Width = dimensionRand;
            fl.AddView(picButtons[picButtons.Count - 1], prms);
            picButtons[picButtons.Count - 1].LongClick += (sender1, e1) =>
            {
                if (Math.Abs(initialX - endX) < 50 && Math.Abs(initialX - endX) < 50)
                {

                    invokePic.Dispose();
                    invokePic = new PicFragment(e.PictureText, e.Url, e.PictureCreated);
                    Android.App.FragmentManager transaction = Activity.FragmentManager;
                    invokePic.Show(transaction, "signup fragment");
                }

            };

            Toast.MakeText(Activity, "Picture Memory Created!", ToastLength.Short).Show();
            

        }

        public bool OnTouch(View v, MotionEvent e)
        {

            initialX = v.GetX();
            initialY = v.GetY();


            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _viewX = v.GetX() - e.RawX;
                    _viewY = v.GetY() - e.RawY;

                    break;
                case MotionEventActions.Move:

                    v.Animate()
                        .X(e.RawX + _viewX)
                       .Y(e.RawY + _viewY)
                       .SetDuration(0)
                       .Start();
                    break;

                case MotionEventActions.Up:

                    endX = v.GetX();
                    endY = v.GetY();
                    //v.StartAnimation(AnimationUtils.LoadAnimation(mView.Context, Resource.Animation.rotate_center)); 
                    break;

                default: return false;


            }
            return false;
        }


        public bool OnClick(View v, MotionEvent e)
        {
            return true;
        }

        private int RandomNumber(int min, int max)
        {
            System.Random random = new System.Random();
            return random.Next(min, max);
        }



        public void CreateButtons(MemoryModel model)
        {

            var metrics = Resources.DisplayMetrics;

            { 
            //ADD TEXT MEMORIES
            for (int i = 0; i < model.TextMemTitle.Count; i++)
            {

                prmList.Add(new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
                textButtons.Add(new Button(mView.Context));
                  var  dimensionRand = new Random().Next(100, 200);
                var index = textButtons.IndexOf(textButtons[i]);
                prmList[index].Height = dimensionRand;
                prmList[index].Width = dimensionRand;
                fl.AddView(textButtons[index], prmList[index]);
                    textButtons[index].SetX(new Random().Next(0, metrics.WidthPixels - 100));
                    textButtons[index].SetY(new Random().Next(0, metrics.HeightPixels - 100));
                    textButtons[index].SetBackgroundResource(Resource.Drawable.textMem);
                textButtons[index].SetOnTouchListener(this);
                textButtons[index].LongClick += (o, s) =>
                {
                    if (Math.Abs(initialX - endX) < 50 && Math.Abs(initialX - endX) < 50)
                    {
                        TextFragment dialogInfo = new TextFragment(model.TextMemTitle[index], model.TextMemText[index], model.TextCreatedBy[index]);
                        Android.App.FragmentManager transaction = Activity.FragmentManager;
                        dialogInfo.Show(transaction, "text button " + i);
                        
                    }

                };
            }

            //ADD PIC MEMORIES

            for (int i = 0; i < model.PicMemTitle.Count; i++)
            {
                prmList.Add(new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
                picButtons.Add(new Button(mView.Context));
                   var rand = new Random().Next(100, 200);
                    var index = textButtons.IndexOf(textButtons[i]);
                prmList[i].Height = rand + 20;
                prmList[i].Width = rand + 20;
                fl.AddView(picButtons[i], prmList[i]);
                    picButtons[index].SetX(new Random().Next(0, metrics.WidthPixels - 100));
                    picButtons[index].SetY(new Random().Next(0, metrics.HeightPixels - 100));
                    picButtons[i].SetBackgroundResource(Resource.Drawable.vidMem);
                picButtons[i].SetOnTouchListener(this);
                picButtons[i].LongClick += (o, s) =>
                {
                    if (Math.Abs(initialX - endX) < 50 && Math.Abs(initialX - endX) < 50)
                    {
                        PicFragment dialogInfo = new PicFragment(model.PicMemTitle[index], model.PicMemUrl[index], model.PicCreatedBy[index]);
                        Android.App.FragmentManager transaction = Activity.FragmentManager;
                        dialogInfo.Show(transaction, "pic button " + i);
                        
                    }

                };
            }
        }

        }
    }

}


