using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Views;

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
            //Set to default theme.
            SetTheme(Resource.Style.Theme_Dark);
            //Set the view to the "main" layout resource.
            SetContentView(Resource.Layout.Main);
            
            //Start the appropriate activity for each of the buttons on click event. 
            ImageButton btnTuner = FindViewById<ImageButton>(Resource.Id.btnTuner);
            btnTuner.Click += delegate 
            {
                StartActivity(typeof(TunerActivity));
            };

            ImageButton btnTutor = FindViewById<ImageButton>(Resource.Id.btnTutor);
            btnTutor.Click += delegate
            {
                StartActivity(typeof(TutorActivity));
            };

            ImageButton btnPlayer = FindViewById<ImageButton>(Resource.Id.btnPlayer);
            btnPlayer.Click += delegate
            {
                StartActivity(typeof(PlayerActivity));
            };

            ImageButton btnRecorder = FindViewById<ImageButton>(Resource.Id.btnRecorder);
            btnRecorder.Click += delegate
            {
                StartActivity(typeof(RecorderActivity));
            };
        }
    }   
}

