using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Android.Support.V4.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace MemoryBox
{

    [Activity()]
    [Serializable]
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

        public FrameLayout Fl
        {
            get
            {
                return fl;
            }

            set
            {
                fl = value;
            }
        }

        public float ViewX
        {
            get
            {
                return _viewX;
            }

            set
            {
                _viewX = value;
            }
        }

        public float ViewY
        {
            get
            {
                return _viewY;
            }

            set
            {
                _viewY = value;
            }
        }

        public float InitialX
        {
            get
            {
                return initialX;
            }

            set
            {
                initialX = value;
            }
        }

        public float InitialY
        {
            get
            {
                return initialY;
            }

            set
            {
                initialY = value;
            }
        }

        public float EndX
        {
            get
            {
                return endX;
            }

            set
            {
                endX = value;
            }
        }

        public float EndY
        {
            get
            {
                return endY;
            }

            set
            {
                endY = value;
            }
        }

        public ImageView BaseImage
        {
            get
            {
                return baseImage;
            }

            set
            {
                baseImage = value;
            }
        }

        public List<Button> VoiceButtons
        {
            get
            {
                return voiceButtons;
            }

            set
            {
                voiceButtons = value;
            }
        }

        public List<Button> TextButtons
        {
            get
            {
                return textButtons;
            }

            set
            {
                textButtons = value;
            }
        }

        public List<Button> PicButtons
        {
            get
            {
                return picButtons;
            }

            set
            {
                picButtons = value;
            }
        }

        public List<FrameLayout.LayoutParams> PrmList
        {
            get
            {
                return prmList;
            }

            set
            {
                prmList = value;
            }
        }

        public Button CreateText
        {
            get
            {
                return createText;
            }

            set
            {
                createText = value;
            }
        }

        public Button CreatePic
        {
            get
            {
                return createPic;
            }

            set
            {
                createPic = value;
            }
        }

        public Button CreateVoice
        {
            get
            {
                return createVoice;
            }

            set
            {
                createVoice = value;
            }
        }

        internal PicFragment InvokePic
        {
            get
            {
                return invokePic;
            }

            set
            {
                invokePic = value;
            }
        }

        public View MView
        {
            get
            {
                return mView;
            }

            set
            {
                mView = value;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Memories, container, false);
            mView = view;

            prmList = new List<FrameLayout.LayoutParams>();
            fl = view.FindViewById<FrameLayout>(Resource.Id.memFrame);

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
                dialogInfo.onSubmitPic += delegate (object sender2, SubmitPicEventArgs e2)
                {
                    var prms = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.WrapContent);
                    ButtonPicMemories picButtonOne = new ButtonPicMemories(view.Context, e2.Picture, e2.PictureText);
                    picButtonOne.SetBackgroundResource(Resource.Drawable.vidMem);
                    var dimensionRand = RandomNumber(100, 200);
                    prms.Height = dimensionRand;
                    prms.Width = dimensionRand;
                    fl.AddView(picButtonOne, prms);
                    picButtonOne.LongClick += (sender1, e1) =>
                    {
                        if (Math.Abs(picButtonOne.InitialX - picButtonOne.EndX) < 50 && Math.Abs(picButtonOne.InitialY - picButtonOne.EndY) < 50)
                        {

                            invokePic.Dispose();

                            invokePic = new PicFragment(picButtonOne.Picture, picButtonOne.TextMemory);
                            Android.App.FragmentManager transaction1 = Activity.FragmentManager;
                            invokePic.Show(transaction1, "signup fragment");
                        }
                    };
                };
            };

            createVoice.Click += (sender, e) =>
            {
                CreateVoiceFragment dialogInfo = new CreateVoiceFragment();
                Android.App.FragmentManager transaction = Activity.FragmentManager;
                dialogInfo.Show(transaction, "signup fragment");
            };

            CreateButtons(view);

            return view;
        }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }



        private void CreateTextMemory(object sender, SubmitTextEventArgs e)
        {
            var prms = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.WrapContent);
            ButtonTextMemories textButtonOne = new ButtonTextMemories(mView.Context, e.Title, e.Text);
            textButtonOne.SetBackgroundResource(Resource.Drawable.textMem);
            var dimensionRand = RandomNumber(100, 200);
            prms.Height = dimensionRand;
            prms.Width = dimensionRand;
            fl.AddView(textButtonOne, prms);
            textButtonOne.LongClick += (sender1, e1) =>
            {
                if (Math.Abs(textButtonOne.InitialX - textButtonOne.EndX) < 50 && Math.Abs(textButtonOne.InitialY - textButtonOne.EndY) < 50)
                {
                    TextFragment dialogInfo = new TextFragment(textButtonOne.Title, textButtonOne.TextMemory);
                    Android.App.FragmentManager transaction = Activity.FragmentManager;
                    dialogInfo.Show(transaction, "signup fragment");
                }
            };

        }

        private void CreatePicMemory(object sender, SubmitPicEventArgs e)
        {
            var prms = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.WrapContent);
            ButtonPicMemories picButtonOne = new ButtonPicMemories(mView.Context, e.Picture, e.PictureText);
            picButtonOne.SetBackgroundResource(Resource.Drawable.vidMem);
            var dimensionRand = RandomNumber(100, 200);
            prms.Height = dimensionRand;
            prms.Width = dimensionRand;
            fl.AddView(picButtonOne, prms);
            picButtonOne.LongClick += (sender1, e1) =>
            {
                if (Math.Abs(picButtonOne.InitialX - picButtonOne.EndX) < 50 && Math.Abs(picButtonOne.InitialY - picButtonOne.EndY) < 50)
                {

                    invokePic.Dispose();

                    invokePic = new PicFragment(picButtonOne.Picture, picButtonOne.TextMemory);
                    Android.App.FragmentManager transaction = Activity.FragmentManager;
                    invokePic.Show(transaction, "signup fragment");
                }
            };

        }

        public bool OnTouch(View v, MotionEvent e)
        {
            
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



        public void CreateButtons(View view)
        {
            int dimensionRand = 0;
            for (int i = 0; i < 1; i++)
            {
                prmList.Add(new FrameLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.WrapContent));

                voiceButtons.Add(new Button(view.Context));
                textButtons.Add(new Button(view.Context));
                picButtons.Add(new Button(view.Context));

                dimensionRand = RandomNumber(100, 200);

                prmList[i].Height = dimensionRand;
                prmList[i].Width = dimensionRand;

                fl.AddView(voiceButtons[i], prmList[i]);
                fl.AddView(textButtons[i], prmList[i]);
                fl.AddView(picButtons[i], prmList[i]);

                voiceButtons[i].SetBackgroundResource(Resource.Drawable.voiceMem);
                textButtons[i].SetBackgroundResource(Resource.Drawable.textMem);
                picButtons[i].SetBackgroundResource(Resource.Drawable.vidMem);



                voiceButtons[i].SetOnTouchListener(this);
                textButtons[i].SetOnTouchListener(this);
                picButtons[i].SetOnTouchListener(this);




                picButtons[i].LongClick += (o, s) =>
                {
                    if (Math.Abs(initialX - endX) < 50 && Math.Abs(initialX - endX) < 50)
                    {
                        PicFragment dialogInfo = new PicFragment();
                        Android.App.FragmentManager transaction = Activity.FragmentManager;
                        dialogInfo.Show(transaction, "signup fragment");
                    }

                };
                textButtons[i].LongClick += (o, s) =>
                {
                    if (Math.Abs(initialX - endX) < 50 && Math.Abs(initialX - endX) < 50)
                    {
                        TextFragment dialogInfo = new TextFragment("A Tale of Two Cities", "It was the best of times, it was the worst of times, it was the age of wisdom, it was the age of foolishness, it was the epoch of belief, it was the epoch of incredulity, it was the season of Light, it was the season of Darkness, it was the spring of hope, it was the winter of despair, we had everything before us, we had nothing before us, we were all going direct to Heaven, we were all going direct the other way--in short, the period was so far like the present period, that some of its noisiest authorities insisted on its being received, for good or for evil, in the superlative degree of comparison only.");
                        Android.App.FragmentManager transaction = Activity.FragmentManager;
                        dialogInfo.Show(transaction, "signup fragment");
                    }
                };

                voiceButtons[i].LongClick += (o, s) =>
                {

                    if (Math.Abs(initialX - endX) < 50 && Math.Abs(initialX - endX) < 50)
                    {
                        VoiceFragment dialogInfo = new VoiceFragment();
                        Android.App.FragmentManager transaction = Activity.FragmentManager;
                        dialogInfo.Show(transaction, "signup fragment");
                    }
                };





            }
        }




    }

}


