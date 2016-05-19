using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android;
using SharpShowcaseView.Targets;
using SharpShowcaseView;
using Virtual_Guitar_Teacher.Controller.Libraries;
using Android.Content.PM;
using Android.Runtime;
using Android.Preferences;

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
        const int REQUEST_CODE_ASK_PERMISSIONS = 434;
        ISharedPreferences prefs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Set to default theme.
            SetTheme(Resource.Style.Theme_Dark);
            //Set the view to the "main" layout resource.
            SetContentView(Resource.Layout.Main);
            //Get shared preferences for later deciding if ShowcaseView is neccessary or not.
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            //If enviroemnt OS is Marshmallow or newer.
            if (Generic.CanMakeSmores())
            {
                //Check for microphone permission.
                //int hasPermission = (int)CheckSelfPermission(Manifest.Permission.RecordAudio);
                Permission permissionState = PackageManager.CheckPermission(
                    Manifest.Permission.RecordAudio,
                    Manifest.Permission_group.Microphone);
                if (permissionState != Permission.Granted)
                {
                    Generic.ShowMsgBox_OK(this, 
                        GetString(Resource.String.AudioRecordPermissionAlertTitle), 
                        GetString(Resource.String.AudioRecordPermissionAlertMsg));
                    RequestPermissions(new string[] { Manifest.Permission.RecordAudio }, REQUEST_CODE_ASK_PERMISSIONS);
                }
            }

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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == REQUEST_CODE_ASK_PERMISSIONS)
                if (grantResults[0] != Permission.Granted)
                {
                    Generic.ShowMsgBox_OK(this, "Permission Denied",
                        "The application will now exit.");
                    this.Finish();
                }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (prefs.GetBoolean("FirstRun", true))
            {
                var vt_btnTutor = new ViewTarget(Resource.Id.btnTutor, this);
                ShowcaseView.ConfigOptions config = new ShowcaseView.ConfigOptions();
                config.IsOneShot = false;
                config.ShowcaseId = 0;
                ShowcaseView showcaseView_btnTutor = ShowcaseView.InsertShowcaseView(vt_btnTutor, this,
                    "Tutorial", "Use this section to train by playing a series of notes.", config);

                var vt_btnTuner = new ViewTarget(Resource.Id.btnTuner, this);
                config.ShowcaseId = 1;
                ShowcaseView showcaseView_btnTuner = ShowcaseView.InsertShowcaseView(vt_btnTuner, this,
                    "Tuner", "Use this to tune your guitar.", config);

                /*NoneOnShowcaseEventListener nosel = new NoneOnShowcaseEventListener();
                nosel.OnShowcaseViewDidHide(showcaseView_btnTuner);
                showcaseView_btnTutor.SetOnShowcaseEventListener(nosel);*/


                showcaseView_btnTutor.Show();

                prefs.Edit().PutBoolean("FirstRun", false).Commit();
            }
        }
    }   
}

