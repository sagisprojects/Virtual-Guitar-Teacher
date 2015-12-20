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
        /// 82.69? / 2 = 41.345
        ///  2 (B)	    246.94 Hz       B3
        /// 50.94? / 2 = 25.470
        ///  3 (G)	    196.00 Hz       G3
        /// 49.17? / 2 = 24.585
        ///  4 (D)	    146.83 Hz       D3
        /// 36.83? / 2 = 18.415
        ///  5 (A)	    110.00 Hz       A2
        /// 27.59? / 2 = 13.795
        ///  6 (E)	    82.41 Hz        E2
        /// 
        /// The filter is responsible for routing the note that is being recieved to its place.
        /// </summary>
        /// <param name="frequency">The note's frequency represented by a number.</param>
        /// <returns></returns>
        public string NoteFrequencyFilter(float frequency)
        {
            string noteIndication = string.Empty;
            double percentage = 0;
            //Determine frequency ranges:
            //E2
            percentage = GetClosenessPercentage(Chords.E2, Chords.A2, frequency);
            //A2
            percentage = GetClosenessPercentage(Chords.A2, Chords.D3, frequency);
            //D3
            percentage = GetClosenessPercentage(Chords.D3, Chords.G3, frequency);
            //G3
            percentage = GetClosenessPercentage(Chords.G3, Chords.B3, frequency);
            //B3
            percentage = GetClosenessPercentage(Chords.B3, Chords.E4, frequency);
            //E4
            percentage = GetClosenessPercentage(Chords.E4, Chords.A4, frequency);

            //Display perfect pitch indication.
            //The precentage should be the opacity of the displayed text indication.

            /*SetContentView(Resource.Layout.);
            TextView button1 = FindViewById<TextView>(Resource.Id.);*/
            // noteIndication = "You are ", percentage," far from note";
            return noteIndication;
        }
        /// <summary>
        ///  Gets the distance between the users' note to the open chord note by percentage.
        /// </summary>
        /// <param name="openChord">The chord that the range will surround it.</param>
        /// <param name="ratioChord">The chord that will be used as a reference to calculate the difference between both chords.</param>
        /// <param name="frequency">The note that the user hits translated to frequency [Hz].</param>
        /// <returns>How close the users' frequency is from the desired note.</returns>
        private double GetClosenessPercentage(double openChord, double ratioChord, float frequency)
        {
            const double fullAlpha = 1; //A full opacity.
            const int offsetOfSeekBar = 100; //Middle of seek bar.
            double closnessAlpha = 0; //Represents the opacity of the difference (a number between 0 and 1).
            double difference = 0; //Represents how close the frequency is from the mark of the note.
            double closenessToMark = 0; //Closeness to the exact note mark. Number between -1 and 1.
            int tuneBarProgress = 0;


            if (frequency == openChord
              || frequency > GetBottomChordRange(openChord, ratioChord)
              && frequency < GetTopChordRange(openChord, ratioChord))
            {
                //How far are you from the mark?
                if (frequency == openChord)    //Right on note.
                    difference = 0;
                else if (frequency > openChord)
                    difference = frequency - openChord;
                else if (frequency < openChord)
                    difference = openChord - frequency;

                //Calculate the closness to the note's mark.
                closenessToMark = difference / Math.Abs(openChord - ratioChord);
                //Calculate the alpha (opacity) of the note indicator.
                closnessAlpha = fullAlpha - Math.Abs(closenessToMark);
                //Calculate the precentage of closnessToMark and add the offset of 100. Between 0 and 200.
                tuneBarProgress = ((int)closenessToMark * 100) + offsetOfSeekBar;

            }

            return closnessAlpha;
        }

        /// <summary>
        /// Gets the bottom of the range of a given open chord. 
        /// </summary>
        /// <param name="openChord">The chord that the range will surround it.</param>
        /// <param name="ratioChord">The chord that will be used as a reference to calculate the difference between both chords.</param>
        /// <returns>Returns the bottom of the range.</returns>
        private double GetBottomChordRange(double openChord, double ratioChord)
        {
            return GetRangeBorder(openChord, ratioChord, false);
        }

        /// <summary>
        /// Gets the top of the range of a given open chord.
        /// </summary>
        /// <param name="openChord">The chord that the range will surround it.</param>
        /// <param name="ratioChord">The chord that will be used as a reference to calculate the difference between both chords.</param>
        /// <returns>Returns the top of the range.</returns>
        private double GetTopChordRange(double openChord, double ratioChord)
        {
            return GetRangeBorder(openChord, ratioChord, true);
        }

        /// <summary>
        /// Gets the border of the range.
        /// </summary>
        /// <param name="openChord">The chord that the range will surround it.</param>
        /// <param name="ratioChord">The chord that will be used as a reference to calculate the difference between both chords.</param>
        /// <param name="isTop">Defines if the range is at the top.</param>
        /// <returns>Returns the border of the range.</returns>
        private double GetRangeBorder(double openChord, double ratioChord, bool isTop)
        {
            double rangeBorder = 0;
            double difference = Math.Abs(openChord - ratioChord);

            if (isTop)
                rangeBorder = openChord + (difference / 2);
            else
                rangeBorder = openChord - (difference / 2);

            return rangeBorder;

        }

    }
}