using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;

namespace Virtual_Guitar_Teacher
{
    class Tuner : Activity
    {
        //Analoge to Digital signal converter.

        MediaRecorder _recorder;
        string _defaultRecordingFilePath = @"\VGT\Documents\Recordings\";

        //
        public void RecorderListenerInit()
        {
            //Set interval clock to sample sound each second (1000 ms).
            _recorder = new MediaRecorder();
            //Timer tmrRecorderListenerDelay = new Timer("Started", 1000, 1000)
            /*_start.Click += delegate {
                stop.Enabled = !stop.Enabled;
                start.Enabled = !start.Enabled;
                */

            //should be dynamic. Find out what is the correct file extention.
            string _fileName = "001";

                _recorder.SetAudioSource(AudioSource.Mic);
                _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
                _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
                _recorder.SetOutputFile(_defaultRecordingFilePath + _fileName);
                _recorder.Prepare();
                _recorder.Start();
        }

        /// <summary>
        /// String	    Frequency	    Scientific pitch notation
        ///  1 (E)	    329.63 Hz       E4
        /// 82.69∆ / 2 = 41.345
        ///  2 (B)	    246.94 Hz       B3
        /// 50.94∆ / 2 = 25.470
        ///  3 (G)	    196.00 Hz       G3
        /// 49.17∆ / 2 = 24.585
        ///  4 (D)	    146.83 Hz       D3
        /// 36.83∆ / 2 = 18.415
        ///  5 (A)	    110.00 Hz       A2
        /// 27.59∆ / 2 = 13.795
        ///  6 (E)	    82.41 Hz        E2
        /// 
        /// The filter is responsible for routing the note that is being recieved to its place.
        /// </summary>
        /// <param name="frequency">The note's frequency represented by a number.</param>
        /// <returns></returns>
        public string NoteFrequencyFilter(float frequency)
        {
            string noteIndication = string.Empty;
            double diff = 0;

            //Determine frequency ranges:


           
            
            // E2
            if (frequency == OpenStringNotes.E2 || frequency > 68.615 && frequency < 96.205)
            {
                //How much close to the mark are you? (by precentage)
                if (frequency == OpenStringNotes.E2)    //Right on point.
                    diff = 0;
                else if (frequency > OpenStringNotes.E2)
                    diff = frequency - OpenStringNotes.E2;
                else if (frequency < OpenStringNotes.E2)
                    diff = OpenStringNotes.E2 - frequency;

                double closenessPrecentage =  (diff / 13.795) * 100;

                //Display perfect pitch indication.
                //The precentage should be the opacity of the displayed text indication.

                /*SetContentView(Resource.Layout.);
                TextView button1 = FindViewById<TextView>(Resource.Id.);*/
            }


            return noteIndication;
        }
    }
}