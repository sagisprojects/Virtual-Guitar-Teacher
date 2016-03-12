using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ViewPager = Android.Support.V4.View.ViewPager;
using Virtual_Guitar_Teacher.Controller;
using System.Linq;

namespace Virtual_Guitar_Teacher.Activities
{
    [Activity(Label = "Virtual Guitar Teacher", Icon = "@drawable/icon", 
        MainLauncher = true, NoHistory = false, Theme = "@style/Theme.Splash")]
        /* INFO:
            MainLauncher 			- This is the activity that should be launched when the user clicks our icon.
            Icon                	- This controls if this activity has an icon in the application launcher.
            Theme 					- Tell android to use our theme we made for this activity.
            NoHistory 				- Tell android not to put the activity in the 'back stack', 
                                        that is, when the user hits the back button from the real application, 
                                        don't show this activity again.
        */
    public class MainActivity : BasicActivityInitialization
    {
        public static Context appContext;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Start the appropriate activity for each of the buttons on click event. 
            Button btnTuner = FindViewById<Button>(Resource.Id.btnTuner);
            btnTuner.Click += delegate 
            {
                StartActivity(typeof(TunerActivity));
            };

            Button btnTutor = FindViewById<Button>(Resource.Id.btnTutor);
            btnTutor.Click += delegate
            {
                StartActivity(typeof(TutorActivity));
            };

            Button btnPlayer = FindViewById<Button>(Resource.Id.btnPlayer);
            btnPlayer.Click += delegate
            {
                StartActivity(typeof(PlayerActivity));
            };

            Button btnRecorder = FindViewById<Button>(Resource.Id.btnRecorder);
            btnRecorder.Click += delegate
            {
                StartActivity(typeof(RecorderActivity));
            };

            //Set to default theme and the view to the "main" layout resource.
            SetTheme(Resource.Style.Theme_Default);
            SetContentView(Resource.Layout.Main);

            //Display a Toast Message on Clicking the btn_Hello  
            //Toast.MakeText(this, "Hello World", ToastLength.Short).Show();
            //Android.Widget.
        }
    }   
}

