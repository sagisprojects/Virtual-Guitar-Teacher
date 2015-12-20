using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Virtual_Guitar_Teacher
{
    [Activity(Label = "Virtual Guitar Teacher", Icon = "@drawable/icon", 
        MainLauncher = true, NoHistory = true )] //, Theme = "@style/Theme.Splash")]
	/* INFO:
		MainLauncher 			- This is the activity that should be launched when the user clicks our icon.
		MainLauncher = true 	- This controls if this activity has an icon in the application launcher.
		Theme 					- Tell android to use our theme we made for this activity.
		NoHistory 				- Tell android not to put the activity in the 'back stack', 
									that is, when the user hits the back button from the real application, 
									don't show this activity again.
	*/
    public class MainActivity : Activity
    {
        //int count = 1;
        public static Context appContext;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.AddFlags(WindowManagerFlags.ShowWhenLocked); //If device is locked, and soft lock is on, show this window.
            //Window.AddFlags(WindowManagerFlags.DismissKeyguard);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);   //Keep on shining you crazy diamond.
            Window.AddFlags(WindowManagerFlags.Fullscreen);     //Hide all screen decorations (such as the statusbar).
            //Window.AddFlags(WindowManagerFlags.LayoutInScreen); //Hide status bar (clock, antenna reception, battery level).

            RequestedOrientation = Android.Content.PM.ScreenOrientation.SensorLandscape;
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            //SetContentView(Resource.Layout.Tuner);


            /*SeekBar sb = FindViewById<SeekBar>(Resource.Id.TuneBar);
            sb.Progress = 10;*/


            // Get our button from the layout resource,
            // and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.MyButton);

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }
    }
}

