using Object = Java.Lang.Object;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Animation;
using System;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    public static class ExtensionMethods
    {
        public static ViewPropertyAnimator ScrollX(this ViewPropertyAnimator vpa, ImageView imgView, params int[] values)
        {
            ObjectAnimator objAnim = ObjectAnimator.OfInt(imgView, "ScrollX", values);
            objAnim.SetDuration(vpa.Duration / 2); //Divide by 2 so it will take less time.
            objAnim.StartDelay = vpa.StartDelay;
            objAnim.Start();
            return vpa;
        }

        public static ViewPropertyAnimator ScrollY(this ViewPropertyAnimator vpa, ImageView imgView, params int[] values)
        {
            ObjectAnimator objAnim = ObjectAnimator.OfInt(imgView, "ScrollY", values);
            objAnim.SetDuration(vpa.Duration);
            objAnim.StartDelay = vpa.StartDelay;
            objAnim.Start();
            return vpa;
        }
    }
}