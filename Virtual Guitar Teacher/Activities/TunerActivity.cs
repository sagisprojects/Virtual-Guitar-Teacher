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
using System.Threading;
using Virtual_Guitar_Teacher.Controller.Libraries;

namespace Virtual_Guitar_Teacher.Activities
{
    [Activity(Label = "TunerActivity")]
    public class TunerActivity : BasicActivityInitialization
    {
        private Tuner tuner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            tuner = new Tuner();

            //Set appropriate layout.
            SetContentView(Resource.Layout.Tuner);

            //Initialize activity and get the microphone listener thread.
            Thread micThread = base.CreateMicrophoneRecorder();

            //Sign up for finished sampling event.
            //base.micManager.FinishedSampling += MicManager_FinishedSampling;

            //Start the microphone listening thread. 
            micThread.Start();
        }

        protected override void OnMicrophoneFinishedSampling(object sender, FinishedSampalingEventArgs e)
        {
            SeekBar sb = FindViewById<SeekBar>(Resource.Id.TuneBar);
            sb.Progress = (int)e.SoundSample;//.Max();
            //tuner.NoteFrequencyFilter(e.SoundSample);
        }
    }
}