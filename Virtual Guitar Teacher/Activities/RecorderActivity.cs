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
using Android.Util;
using Virtual_Guitar_Teacher.Controller.Libraries;
using System.Threading;

namespace Virtual_Guitar_Teacher.Activities
{
    [Activity(Label = "RecorderActivity", Theme = "@style/Theme.Record")]
    public class RecorderActivity : BasicActivityInitialization
    {
        Recorder _recorder;
        bool hasIntroAnimationFinished = false;
        TextView _closestNote;
        ImageButton _btnRecord;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set appropriate layout.
            SetContentView(Resource.Layout.Recorder);

            _recorder = new Recorder();

            /*GridLayout gridLayout = FindViewById< GridLayout>(Resource.Id.gridLayout);
            gridLayout.Touch += GridLayout_Touch;
            View view = gridLayout.GetChildAt(5);

            this.AddContentView(view, null);

            Log.Info("", view.LayoutParameters.Height.ToString());
            Log.Info("", view.LayoutParameters.Width.ToString());
            Log.Info("", view.GetX().ToString());
            Log.Info("", view.GetY().ToString());*/

            //Initialize activity and get the microphone listener thread.
            Thread micThread = CreateMicrophoneRecorder();

            OnMicrophoneFinishedSampling += RecorderActivity_OnMicrophoneFinishedSampling;

            //Start the microphone listening thread.
            //micThread.Start();

            _closestNote = FindViewById<TextView>(Resource.Id.closestNote);

            ImageView guitarBG = FindViewById<ImageView>(Resource.Id.guitarBG);
            //guitarBG.SetImageResource(Resource.Drawable.Neck_Acustic_01);

            _recorder.AnimateGuitarIntro(guitarBG);
            _recorder.OnIntroAnimationFinished += Recorder_OnIntroAnimationFinished; ;

            //Add test recording list item.
            ListView lstRecordings = FindViewById< ListView>(Resource.Id.lstRecordings);
            TextView txtRec = new TextView(this);
            txtRec.Text = "Test recording";
            lstRecordings.AddView(txtRec);
            lstRecordings.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => 
            {
                Toast.MakeText(this,((TextView)e.View).Text, ToastLength.Long);
            };

            //On record button click.
            _btnRecord = FindViewById<ImageButton>(Resource.Id.btnRecord);
            _btnRecord.Click += (object sender, EventArgs e) => 
            {
                micManager.Record();
                //TODO: Switch button to stop recording.
            };
        }

        private void RecorderActivity_OnMicrophoneFinishedSampling(object sender, FinishedSampalingEventArgs e)
        {
            //Get closest note and closness as an angle.
            Note closestNote = _recorder.FindClosestNote(e.Frequency);//it's constantly recording, how will it know that there's a legit note detected?
            
            RunOnUiThread(new Action(() =>
                {
                    //_txtFrequency.Text = e.Frequency.ToString();
                    //Set note text.
                    _closestNote.Text = closestNote.Name;

                    _recorder.SpotAnimateAllPositionsOfANote(closestNote);
                }
            ));
        }

        private void Recorder_OnIntroAnimationFinished(object sender, EventArgs e)
        {
            hasIntroAnimationFinished = true;
            OnWindowFocusChanged(true);
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus && hasIntroAnimationFinished 
                && _btnRecord.Visibility != ViewStates.Visible
                && _closestNote.Visibility != ViewStates.Visible)
            {
                _btnRecord.Visibility = ViewStates.Visible;
                _closestNote.Visibility = ViewStates.Visible;
            }
        }

        /*private void GridLayout_Touch(object sender, View.TouchEventArgs e)
        {
            long duration = e.Event.DownTime;
            long delay = e.Event.EventTime;
            float x = e.Event.XPrecision;
            float y = e.Event.YPrecision;

            Log.Info("", duration.ToString());
            Log.Info("", delay.ToString());
            Log.Info("", x.ToString());
            Log.Info("", y.ToString());
        }*/
    }
}