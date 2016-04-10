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
using Virtual_Guitar_Teacher.Controller;
using Android.Util;

namespace Virtual_Guitar_Teacher.Activities
{
    [Activity(Label = "RecorderActivity")]
    public class RecorderActivity : BasicActivityInitialization
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set appropriate layout.
            SetContentView(Resource.Layout.Recorder);

            Recorder record = new Recorder();

            GridLayout gridLayout = FindViewById< GridLayout>(Resource.Id.gridLayout);
            gridLayout.Touch += GridLayout_Touch;
            View view = gridLayout.GetChildAt(5);

            this.AddContentView(view, null);

            Log.Info("", view.LayoutParameters.Height.ToString());
            Log.Info("", view.LayoutParameters.Width.ToString());
            Log.Info("", view.GetX().ToString());
            Log.Info("", view.GetY().ToString());
        }

        private void GridLayout_Touch(object sender, View.TouchEventArgs e)
        {
            long duration = e.Event.DownTime;
            long delay = e.Event.EventTime;
            float x = e.Event.XPrecision;
            float y = e.Event.YPrecision;

            Log.Info("", duration.ToString());
            Log.Info("", delay.ToString());
            Log.Info("", x.ToString());
            Log.Info("", y.ToString());
        }
    }
}