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

namespace Virtual_Guitar_Teacher
{
    [Activity(Label = "SplashActivity")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            /*requestFeature(Window.FEATURE_ACTION_BAR);
			ActionBar.hide();*/
            SetContentView(Resource.Layout.Splash);
        }
    }
}