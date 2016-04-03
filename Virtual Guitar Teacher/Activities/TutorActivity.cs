using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Threading;
using Virtual_Guitar_Teacher.Controller;
using Virtual_Guitar_Teacher.Controller.Libraries;
using Android.Graphics.Drawables;

namespace Virtual_Guitar_Teacher.Activities
{
    [Activity(Label = "TutorActivity", Theme = "@style/Theme.Light")]
    public class TutorActivity : BasicActivityInitialization
    {
        Tutor tutor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            tutor = new Tutor(this);

            //Set appropriate layout.
            SetContentView(Resource.Layout.Tutor);

            //Initialize activity and get the microphone listener thread.
            Thread micThread = CreateMicrophoneRecorder();

            OnMicrophoneFinishedSampling += TutorActivity_OnMicrophoneFinishedSampling;

            //Start the microphone listening thread.
            //micThread.Start();

            ImageView guitarBG = FindViewById<ImageView>(Resource.Id.guitarBG);
            //guitarBG.SetImageResource(Resource.Drawable.Neck_Acustic_01);

            tutor.AnimateGuitarIntro(guitarBG);

            Log.Info("TutorActivity","OnCreate");
            tutor.StartTutoring();
        }

        private void TutorActivity_OnMicrophoneFinishedSampling(object sender, FinishedSampalingEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}