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
using Virtual_Guitar_Teacher.Controller.Libraries;
using System.Threading;

namespace Virtual_Guitar_Teacher.Activities
{
    [Activity(Label = "PlayerActivity", Theme = "@style/Theme.Play")]
    public class PlayerActivity : BasicActivityInitialization
    {
        Player _player;
        bool hasSongStartedPlaying = false;
        bool hasIntroAnimationFinished = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set appropriate layout.
            SetContentView(Resource.Layout.Player);

            _player = new Player(this);

            //Initialize activity and get the microphone listener thread.
            Thread micThread = CreateMicrophoneRecorder();

            OnMicrophoneFinishedSampling += PlayerActivity_OnMicrophoneFinishedSampling;

            //Start the microphone listening thread.
            //micThread.Start();

            ImageView guitarBG = FindViewById<ImageView>(Resource.Id.guitarBG);
            //guitarBG.SetImageResource(Resource.Drawable.Neck_Acustic_01);

            _player.AnimateGuitarIntro(guitarBG);
            _player.OnIntroAnimationFinished += Player_OnIntroAnimationFinished;

            //_player.ReadSong();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus && !hasSongStartedPlaying && hasIntroAnimationFinished)
            {
                hasSongStartedPlaying = true;
                //_player.PlaySong();
            }
        }

        private void Player_OnIntroAnimationFinished(object sender, EventArgs e)
        {
            hasIntroAnimationFinished = true;
            OnWindowFocusChanged(true);
        }

        private void PlayerActivity_OnMicrophoneFinishedSampling(object sender, FinishedSampalingEventArgs e)
        {
            _player.CurrentlyPlayedFrequency = e.Frequency;
        }
    }
}