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
using Android.Util;

namespace MemoryBox
{
    class ButtonTextMemories : Button, View.IOnTouchListener
    {
        private string title;
        private string text;
        private Context ctx;
        private float initialX, initialY, _viewX, _viewY, endX, endY;

        public string Title
        {
            get { return title;  }
            set { title = value; }
        }

        public string TextMemory
        {
            get { return text; }
            set { text = value; }
        }

        public float InitialX
        {
            get { return initialX; }
            set { initialX = value; }
        }

        public float InitialY
        {
            get { return initialY; }
            set { initialY = value; }
        }

        public float EndX
        {
            get { return endX; }
            set { endX = value; }
        }

        public float EndY
        {
            get { return endY; }
            set { endY = value; }
        }

        public ButtonTextMemories (Context context,  string infTitle, string infText) : base(context)
        {
            title = infTitle;
            text = infText;
            ctx = context;
            this.SetHeight(50);
            this.SetWidth(50);
            SetOnTouchListener(this);
            

        }

        public bool OnTouch(View v, MotionEvent e)
        {

            initialX = v.GetX();
            initialY = v.GetY();
            InitialX = initialX;
            InitialY = initialY;

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
                    EndX = endX;
                    EndY = endY;
                    break;

                default: return false;


            }
            return false;
        }
    }
}