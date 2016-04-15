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
    [Activity(Label = "TunerActivity", Theme = "@style/Theme.Tune")]
    public class TunerActivity : BasicActivityInitialization
    {
        private Tuner tuner;
        SeekBar _volumeBar;
        TextView _closestNote;
        ImageView _frequencyIndicator;
        ImageView _frequencyGauge;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set appropriate layout.
            SetContentView(Resource.Layout.Tuner);

            tuner = new Tuner();

            //Initialize activity and get the microphone listener thread.
            Thread micThread = CreateMicrophoneRecorder();

            OnMicrophoneFinishedSampling += TunerActivity_OnMicrophoneFinishedSampling;

            //Get screen controls.
            _volumeBar = FindViewById<SeekBar>(Resource.Id.volumeBar);
            _closestNote = FindViewById<TextView>(Resource.Id.closestNote);
            _frequencyIndicator = FindViewById<ImageView>(Resource.Id.frequencyIndicator);
            _frequencyGauge = FindViewById<ImageView>(Resource.Id.frequencyGauge);

            //Set special position for frequencyIndicator, which is the middle of the screen.
            int haflTheHeightOfTheScreen = Generic.GetScreenDimentions(this).HeightPixels / 2;
            _frequencyIndicator.SetY(haflTheHeightOfTheScreen);
            _frequencyGauge.SetY(haflTheHeightOfTheScreen);

            //Start the microphone listening thread. 
            micThread.Start();
        }

        private void TunerActivity_OnMicrophoneFinishedSampling(object sender, FinishedSampalingEventArgs e)
        {
            //Display volume level.
            _volumeBar.Progress = (int)e.Volume;

            //Get closest note and closness as an angle.
            Tuner.NoteDifference noteDiff = tuner.NoteFrequencyFilter(e.Frequency);

            RunOnUiThread(new Action(() => 
            {
                //Set note text.
                _closestNote.Text = noteDiff.ClosestNote;
                //Set dial angle.
                _frequencyIndicator.Rotation = noteDiff.ClosnessByPercentage_Base90;
            }
            ));
        }
    }
}