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
using Android.Graphics;
using System.Threading;
using Virtual_Guitar_Teacher.Controller;
using Virtual_Guitar_Teacher.Controller.Libraries;

namespace Virtual_Guitar_Teacher.Activities
{
    [Activity(Label = "TutorActivity")]
    public class TutorActivity : BasicActivityInitialization
    {
        Tutor tutor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            tutor = new Tutor();

            //Set appropriate layout.
            SetContentView(Resource.Layout.Tutor);

            //Initialize activity and get the microphone listener thread.
            Thread micThread = base.CreateMicrophoneRecorder();

            //Start the microphone listening thread. 
            //micThread.Start();

            AnimateGuitarIntro();
        }

        private void AnimateGuitarIntro()
        {
            ImageView guitarBG = FindViewById<ImageView>(Resource.Id.guitarBG);
            int initialScrollX = guitarBG.ScrollX;
            int targetScrollX = 470; //dp

            for (int x = initialScrollX; x > targetScrollX; x--)
            {
                guitarBG.ScrollX = x;
                Thread.Sleep(10);
            }
        }

        protected override void OnMicrophoneFinishedSampling(object sender, FinishedSampalingEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}