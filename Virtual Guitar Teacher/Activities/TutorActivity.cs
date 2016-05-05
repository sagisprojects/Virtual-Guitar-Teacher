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
using System.Reflection;

namespace Virtual_Guitar_Teacher.Activities
{
    [Activity(Label = "TutorActivity", Theme = "@style/Theme.Light")]
    public class TutorActivity : BasicActivityInitialization
    {
		Tutor _tutor;
        bool hasSongStartedPlaying = false;
        bool hasIntroAnimationFinished = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set appropriate layout.
            SetContentView(Resource.Layout.Tutor);



            OnMicrophoneFinishedSampling += TutorActivity_OnMicrophoneFinishedSampling;

            //Start the microphone listening thread.
            //micThread.Start();

            //TableLayout tableLayout = FindViewById<TableLayout>(Resource.Id.tableLayout);
            //tableLayout.Touch += TableLayout_Touch;

            //SetTableControls(tableLayout);
        }

        protected override void OnStart()
        {
            Log.Info("TutorActivity", "OnStart");
            base.OnStart();

            _tutor = new Tutor(this);

            ImageView guitarBG = FindViewById<ImageView>(Resource.Id.guitarBG);
            //guitarBG.SetImageResource(Resource.Drawable.Neck_Acustic_01);

            _tutor.AnimateGuitarIntro(guitarBG);
            _tutor.OnIntroAnimationFinished += Tutor_OnIntroAnimationFinished;

            _tutor.ReadSong();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        /*private void SetTableControls(TableLayout tableLayout)
        {
            TextView textView = new TextView(this);
            Spinner spinner = new Spinner(this);

            Dictionary<string, string> tableProperties = GetPropertiesOf(tableLayout);
            Dictionary<string, string>.KeyCollection keys = tableProperties.Keys;
            List<string> keysList = keys.ToList();

            ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(this, spinner.Id, keysList);
            arrayAdapter.SetDropDownViewResource(spinner.Id);
            spinner.Adapter = arrayAdapter;

            spinner.SetX(0);
            spinner.SetY(0);

            this.AddContentView(spinner, new ViewGroup.LayoutParams(100, 25));
        }

        Dictionary<string, string> GetPropertiesOf(object obj)
        {
            var props = obj.GetType().GetProperties();
            Dictionary<string, string> dictionary = new Dictionary<string, string>(props.Length);

            for (int i = 0; i < props.Length; i++)
            {
                dictionary.Add(props[i].Name, props[i].GetValue(props[i]).ToString());
            }
            return dictionary;
        }

        private void TableLayout_Touch(object sender, View.TouchEventArgs e)
        {
            Log.Info("", e.Event.GetX().ToString());
            Log.Info("", e.Event.GetY().ToString());
        }*/

        private void Tutor_OnIntroAnimationFinished(object sender, EventArgs e)
        {
            hasIntroAnimationFinished = true;
            //Display all strings colors in the view.
            _tutor.ShowStringsColors(FindViewById<TableLayout>(Resource.Id.stingsColors));
            //Play song.
            OnWindowFocusChanged(true);
        }

        /*protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            bool isShown;
            View view = FindViewById(Resource.Id.fret1);
            if (view != null)
                isShown = view.IsShown;
        }

        protected override void OnStart()
        {
            base.OnStart();
            bool isShown;
            View view = FindViewById(Resource.Id.fret1);
            if (view != null)
                isShown = view.IsShown;
        }

        public override View OnCreateView(string name, Context context, IAttributeSet attrs)
        {
            bool isShown;
            View view = FindViewById(Resource.Id.fret1);
            if (view != null)
                isShown = view.IsShown;
            return base.OnCreateView(name, context, attrs);
        }*/

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus && !hasSongStartedPlaying && hasIntroAnimationFinished)
            {
                hasSongStartedPlaying = true;
                _tutor.PlaySong();
            }
        }

        private void TutorActivity_OnMicrophoneFinishedSampling(object sender, FinishedSampalingEventArgs e)
        {
			_tutor.CurrentlyPlayedFrequency = e.Frequency;
        }
    }
}