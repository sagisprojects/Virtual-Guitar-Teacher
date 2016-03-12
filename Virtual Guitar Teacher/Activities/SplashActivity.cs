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
using System.Threading;

namespace Virtual_Guitar_Teacher.Activities
{
    [Activity(Label = "Virtual Guitar Teacher", MainLauncher = true, 
        Theme = "@style/Theme.Splash", NoHistory = true, Icon = "@drawable/icon")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Display Splash Screen for 4 Sec
            Thread.Sleep(4000);
            //Do some pre proccessing and resources loading.
            //Start MainActivity Activity
            StartActivity(typeof(MainActivity));

            /*requestFeature(Window.FEATURE_ACTION_BAR);
			ActionBar.hide();
            SetContentView(Resource.Layout.Splash);*/
        }
    }
}