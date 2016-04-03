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
            Thread micThread = CreateMicrophoneRecorder();

            OnMicrophoneFinishedSampling += TunerActivity_OnMicrophoneFinishedSampling;     

            //Start the microphone listening thread. 
            micThread.Start();
        }

        private void TunerActivity_OnMicrophoneFinishedSampling(object sender, FinishedSampalingEventArgs e)
        {
            SeekBar tuneBar = FindViewById<SeekBar>(Resource.Id.tuneBar);
            tuneBar.Progress = (int)e.Frequency.CyclesPerSecond;

            RunOnUiThread(new Action(() => 
            {
                TextView closestNote = FindViewById<TextView>(Resource.Id.closestNote);
                closestNote.Text = e.Frequency.CyclesPerSecond + " Hz";
            }
            ));
            //tuner.NoteFrequencyFilter(e.SoundSample);
        }
    }
}