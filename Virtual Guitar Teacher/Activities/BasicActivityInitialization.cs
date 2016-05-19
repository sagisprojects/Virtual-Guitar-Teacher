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
using Virtual_Guitar_Teacher.Controller.Libraries;

namespace Virtual_Guitar_Teacher.Activities
{
    /// <summary>
    /// Sets default settings for the activity,
    /// and hndles the creation of the microphone listener thread.
    /// </summary>
    public class BasicActivityInitialization : Activity
    {
        protected volatile bool shouldListen = true;
        protected MicrophoneManager micManager;
        private Thread microphoneThread;
        /// <summary>
        /// Fires once a sample of the microphone input is ready for use.
        /// </summary>
        protected event FinishedSamplingEventHandler OnMicrophoneFinishedSampling;

        /// <summary>
        /// Initializes application's window basic flags, orientation, and hides the title bar.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set window flags.
            Window.AddFlags(WindowManagerFlags.ShowWhenLocked); //If device is locked, and soft lock is on, show this window.
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);   //Keep on shining you crazy diamond.
            Window.AddFlags(WindowManagerFlags.Fullscreen);     //Hide all screen decorations (such as the statusbar).
            //Window.AddFlags(WindowManagerFlags.DismissKeyguard);
            //Window.AddFlags(WindowManagerFlags.LayoutInScreen); //Hide status bar (clock, antenna reception, battery level).

            RequestedOrientation = Android.Content.PM.ScreenOrientation.SensorLandscape;
            RequestWindowFeature(WindowFeatures.NoTitle);
        }

        /// <summary>
        /// Initializes a new thread to be used for constantly listening to the microphone input.
        /// </summary>
        /// <returns>The thread which the microphone listener is running on.</returns>
        protected Thread CreateMicrophoneRecorder()
        {
            //Microphone initialization;
            micManager = new MicrophoneManager();
            micManager.FinishedSampling += OnMicrophoneFinishedSamplingEvent;

            //Crate new thread for constantly listening to the microphone input.
            Thread thread = new Thread(new ThreadStart(() => 
            {
                //Initialize microphone recorder.
                micManager.RecorderInit();
                //Listen constantly, untill a further notice.
                while (shouldListen)
                {
                    micManager.Listen();
                }
            }));

            return thread;
        }

        private void OnMicrophoneFinishedSamplingEvent(object sender, FinishedSampalingEventArgs e)
        {
            OnMicrophoneFinishedSampling(sender, e);
        }

        protected override void OnStart()
        {
            base.OnStart();
            shouldListen = true;

            //Initialize activity and get the microphone listener thread.
            microphoneThread = CreateMicrophoneRecorder();

            /*if (microphoneThread.ThreadState != ThreadState.Running
                && OnMicrophoneFinishedSampling != null)
                microphoneThread.Start();*/
        }

        protected override void OnPause()
        {
            base.OnPause();
            shouldListen = false;
            //microphoneThread.Suspend(); //Abort?
        }

        protected override void OnStop()
        {
            base.OnStop();
            shouldListen = false;
        }

        protected override void OnResume()
        {
            base.OnResume();
            shouldListen = true;
            /*if (microphoneThread.ThreadState != ThreadState.Running
                && OnMicrophoneFinishedSampling != null)
                microphoneThread.Start();*/
        }
        /// <summary>
        /// Order the microphone listener thread to stop listening, and exit naturally,
        /// upon a back button press.
        /// </summary>
        public override void OnBackPressed()
        {
            shouldListen = false;
            base.OnBackPressed();
        }

        /// <summary>
        /// Order the microphone listener thread to stop listening, and exit naturally,
        /// once this activity is destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            shouldListen = false;
            base.OnDestroy();
        }

        /// <summary>
        /// Order the microphone listener thread to stop listening, and exit naturally,
        /// once this activity is no longer needed by the application.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            shouldListen = false;
            base.Dispose(disposing);
        }
    }
}