using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    class GraphicsDrawer : SurfaceView, ISurfaceHolder
    {
        public GraphicsDrawer(Context context) : base(context)
        {
        }

        public GraphicsDrawer(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public GraphicsDrawer(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        protected GraphicsDrawer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public bool IsCreating
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Surface Surface
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Rect SurfaceFrame
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AddCallback(ISurfaceHolderCallback callback)
        {
            throw new NotImplementedException();
        }

        public Canvas LockCanvas()
        {
            throw new NotImplementedException();
        }

        public Canvas LockCanvas(Rect dirty)
        {
            throw new NotImplementedException();
        }

        public void RemoveCallback(ISurfaceHolderCallback callback)
        {
            throw new NotImplementedException();
        }

        public void SetFixedSize(int width, int height)
        {
            throw new NotImplementedException();
        }

        public void SetFormat([GeneratedEnum] Format format)
        {
            throw new NotImplementedException();
        }

        public void SetKeepScreenOn(bool screenOn)
        {
            throw new NotImplementedException();
        }

        public void SetSizeFromLayout()
        {
            throw new NotImplementedException();
        }

        public void SetType([GeneratedEnum] SurfaceType type)
        {
            throw new NotImplementedException();
        }

        public void UnlockCanvasAndPost(Canvas canvas)
        {
            throw new NotImplementedException();
        }
    }
}