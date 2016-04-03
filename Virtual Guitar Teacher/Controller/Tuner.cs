using System;
using Virtual_Guitar_Teacher.Controller.Libraries;

namespace Virtual_Guitar_Teacher.Controller
{
    class Tuner
    {
        /// <summary>
        /// Open string notes:
        /// String	    Frequency	    Scientific pitch notation
        ///  1 (E)	    329.63 Hz       E4
        /// 82.69? / 2 = 41.345
        ///  2 (B)	    246.94 Hz       B3
        /// 50.94? / 2 = 25.470
        ///  3 (G)	    196.00 Hz       G3
        /// 49.17? / 2 = 24.585
        ///  4 (D)	    146.83 Hz       D3
        /// 36.83? / 2 = 18.415
        ///  5 (A)	    110.000 Hz       A2
        /// 27.59? / 2 = 13.795
        ///  6 (E)	    82.407 Hz        E2
        /// 
        /// The filter is responsible for routing the note that is being recieved to its place.
        /// </summary>
        /// <param name="frequency">The note's frequency represented by a number.</param>
        /// <returns></returns>
        public NoteDifference NoteFrequencyFilter(float frequency)
        {
            //Determine frequency range. (Get 2 closest open notes that the frequency is between them).
            float[] twoClosestOpenNotes = GetTwoClosestOpenNotes(frequency);

            //Find out to which of the two closest notes the played frequency is closer to.
            float closestNote = FindClosestNote(twoClosestOpenNotes, frequency); 

            //Find out how close the played frequency is from its closest note by percetage.
            NoteDifference noteIndication = CalculateRatioOfCloseness(twoClosestOpenNotes, closestNote, frequency);

            //Get the closest note for presentation.
            string noteNameRepresentation = string.Empty;
         //   Frequencies.OpenStrings.notes.TryGetValue(closestNote, out notePresentation);

            noteIndication.ClosestNote = noteNameRepresentation;

            return noteIndication;
        }

        /*private const float E2 = 82.407f;
        private const float A2 = 110.000f;
        private const float D3 = 146.832f;
        private const float G3 = 184.997f;
        private const float B3 = 246.942f;
        private const float E4 = 329.628f;*/

        /// <summary>
        /// Finds the two closest open chord notes (as showen at NoteFrequencyFilter description) 
        /// of the played frequency.
        /// </summary>
        /// <param name="playedFrequency">The frequency the user played.</param>
        /// <returns>Returns the two closest open chord notes.</returns>
        private float[] GetTwoClosestOpenNotes(float playedFrequency)
        {
            float[] twoClosestOpenNotes = new float[2];  //Create a 2 units array of type NoteFrequencies.
            float[] differences = new float[6];          //Creat an array of differences between the playedFrequency to each of the open notes.

            if (playedFrequency <= OpenStrings.E2)
            {
                twoClosestOpenNotes[0] = OpenStrings.E2;
                twoClosestOpenNotes[1] = 0;
            }
            else if (playedFrequency > OpenStrings.E2 && playedFrequency <= OpenStrings.A2)
            {
                twoClosestOpenNotes[0] = OpenStrings.E2;
                twoClosestOpenNotes[1] = OpenStrings.A2;
            }
            else if (playedFrequency > OpenStrings.A2 && playedFrequency <= OpenStrings.D3)
            {
                twoClosestOpenNotes[0] = OpenStrings.A2;
                twoClosestOpenNotes[1] = OpenStrings.D3;
            }
            else if (playedFrequency > OpenStrings.D3 && playedFrequency <= OpenStrings.G3)
            {
                twoClosestOpenNotes[0] = OpenStrings.D3;
                twoClosestOpenNotes[1] = OpenStrings.G3;
            }
            else if (playedFrequency > OpenStrings.G3 && playedFrequency <= OpenStrings.B3)
            {
                twoClosestOpenNotes[0] = OpenStrings.G3;
                twoClosestOpenNotes[1] = OpenStrings.B3;
            }
            else if (playedFrequency > OpenStrings.B3 && playedFrequency <= OpenStrings.E4)
            {
                twoClosestOpenNotes[0] = OpenStrings.B3;
                twoClosestOpenNotes[1] = OpenStrings.E4;
            }
            else if (playedFrequency > OpenStrings.E4)
            {
                twoClosestOpenNotes[0] = OpenStrings.E4;
                twoClosestOpenNotes[1] = 0;
            }
        
            return twoClosestOpenNotes;
        }

        /// <summary>
        /// Finds the main note that is being tuned to.
        /// </summary>
        /// <param name="twoClosestOpenNotes">The two open notes that the frequency id between them.</param>
        /// <param name="frequency">The given frequency.</param>
        /// <returns>Retruns the main note that is being tuned to.</returns>
        private float FindClosestNote(float[] twoClosestOpenNotes, float frequency)
        {
            if ((twoClosestOpenNotes[0] - frequency) < (frequency - twoClosestOpenNotes[1]))
                return twoClosestOpenNotes[0];
            else
                return twoClosestOpenNotes[1];
        }

        /// <summary>
        ///  Gets the distance between the played frequency to the open note by percentage.
        /// </summary>
        /// <param name="closestNote">The two closest notes that will be used as a reference to calculate the difference between both notes.</param>
        /// <param name="playedFrequency">The note that the user hits translated to frequency [Hz].</param>
        /// <returns>How close the users' frequency is from the desired note.</returns>
        private NoteDifference CalculateRatioOfCloseness(float[] twoClosestOpenNotes, float closestNote, float playedFrequency)
        {
            float closenessToMark = 0; //Closeness to the exact note mark. Number between 0 and 1.
            NoteDifference noteDiff = new NoteDifference();

            //Represents how close the frequency is from the closestNote.
            float difference = Math.Abs(playedFrequency - closestNote); 
            
            float middleOfTwoOpenNotes = Math.Abs(twoClosestOpenNotes[1] - twoClosestOpenNotes[0]) / 2;

            //Calculate the closness to the closestNote.
            closenessToMark = difference / middleOfTwoOpenNotes;
            //Calculate the alpha (opacity) of the note indicator.
            noteDiff.ClosnessAlpha = NoteDifference.FULL_ALPHA - closenessToMark;
            //Calculate the precentage of closnessToMark and add the offset of 100. Between 0 and 256.
            noteDiff.TuneBarProgress = (int)(closenessToMark * 100) + NoteDifference.SEEKBAR_OFFSET;
            
            return noteDiff;
        }

        /// <summary>
        /// Defines the difference (By precentage and opacity) of the played note 
        /// from the desired note (closest), and a representaion of the closest note.
        /// </summary>
        public struct NoteDifference
        {
            //Constants:
            public const double FULL_ALPHA = 1; //A full opacity.
            public const int SEEKBAR_OFFSET = 128; //Middle of seek bar.

            //Properties:
            /// <summary>
            ///Represents the opacity of the difference (a number between 0 and 1).
            /// </summary>
            public double ClosnessAlpha { get; set; }
            /// <summary>
            /// Represents the bar progresses by a number.
            /// </summary>
            public int TuneBarProgress { get; set; }
            /// <summary>
            /// Represents the closest open string note as a string.
            /// </summary>
            public string ClosestNote { get; set; }
        }
    }
}