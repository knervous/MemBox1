using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MemoryBox
{
    [Activity(Label = "MemoryBox", MainLauncher = true, Icon = "@drawable/memBox")]
    public class MainActivity : Activity, View.IOnTouchListener
    {
        
        private Button myButton;
        private float _viewX;
        private float _viewY;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            SetContentView(Resource.Layout.Main);
            
            myButton = FindViewById<Button>(Resource.Id.MyButton);
            myButton.SetOnTouchListener(this);
        }

        //public bool OnTouch(View v, MotionEvent e)
        //{
        //    switch (e.Action)
        //    {
        //        case MotionEventActions.Down:
        //            _viewX = e.GetX();
        //            _viewY = e.GetY();
        //            v.Visibility = ViewStates.Visible;
        //            break;

        //        case MotionEventActions.Move:
        //            var left = (int)(e.RawX - _viewX);
        //            var right = (int)(left + v.Width);
        //            var top = (int)(e.RawY - _viewY);
        //            var bottom = (int)(top - v.Height);
        //            v.Layout(left, top, right, bottom);
        //            v.Visibility = ViewStates.Visible;
        //            break;

        //        case MotionEventActions.Up:
        //            v.Visibility = ViewStates.Visible;
        //            break;
        //    }
        //    v.Visibility = ViewStates.Visible;
        //    return true;
        //}

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _viewX = v.GetX() - e.RawX;
                    _viewY = v.GetY() - e.RawY;

                    break;
                case MotionEventActions.Move:
                    //var left = (int)(e.RawX - _viewX);
                    //var right = (int)(left + v.Width);
                    //v.Layout(left, v.Top, right, v.Bottom);
                    //break;
                    v.Animate()
                        .X(e.RawX + _viewX)
                       .Y(e.RawY + _viewY)
                       .SetDuration(0)
                       .Start();
                    break;
                default: return false;

            }
            return true;
        }
    }
}

