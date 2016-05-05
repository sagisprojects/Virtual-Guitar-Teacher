using System;
using Virtual_Guitar_Teacher.Controller.Libraries;

namespace Virtual_Guitar_Teacher.Controller
{
    class Tuner
    {
        /// <summary>
        /// Defines the difference (by precentage and opacity) of the played note 
        /// from the desired note (closest), and a representaion of the closest note.
        /// </summary>
        public struct NoteDifference
        {
            //Constants:
            //public const double FULL_ALPHA = 1; //A full opacity.
            //public const int SEEKBAR_OFFSET = 128; //Middle of seek bar.

            //Properties:
            /// <summary>
            ///Represents the opacity of the difference (a number between 0 and 1).
            /// </summary>
            //public double ClosnessAlpha { get; set; }
            /// <summary>
            /// Represents the precentage of closness by base 90.
            /// </summary>
            public float ClosnessByPercentage_Base90 { get; set; }
            /// <summary>
            /// Represents the closest open string note as a string.
            /// </summary>
            public string ClosestNote { get; set; }
        }

        /// <summary>
        /// Defines an upper and a lower note pair.
        /// </summary>
        private struct UpperAndLowerNotes
        {
            Note _upper, _lower;

            public UpperAndLowerNotes(Note lower, Note upper)
            {
                //Initial values.
                _upper = upper;
                _lower = lower;
            }

            public Note Upper
            {
                get { return _upper; }
                set
                {
                    if (value.Hertz < _lower.Hertz)
                        throw new Exception("Upper value cannot be less than lower value.");
                    else
                        _upper = value;
                }
            }
            public Note Lower
            {
                get { return _lower; }
                set
                {
                    if (value.Hertz > _upper.Hertz)
                        throw new Exception("Lower value cannot be greater than upper value.");
                    else
                        _lower = value;
                }
            }
        }

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
        public NoteDifference NoteFrequencyFilter(Hz frequency)
        {
            //Determine frequency range. (Get 2 closest open notes that the frequency is between them).
            UpperAndLowerNotes twoClosestOpenNotes = GetTwoClosestOpenNotes(frequency);

            //Assume a closest note.
            Note closestNote = twoClosestOpenNotes.Upper;

            //If the two closest notes are actually diffrent ones, then...
            if (twoClosestOpenNotes.Upper != twoClosestOpenNotes.Lower)
                //Find which out of the two closest notes the played frequency is closer to.
                closestNote = FindClosestNote(twoClosestOpenNotes, frequency); 

            //Find out how close the played frequency is from its closest note by percetage.
            //NoteDifference noteDifference = 
            return CalculateRatioOfCloseness(twoClosestOpenNotes, closestNote, frequency);

            //Get the closest note for presentation.
            //string noteNameRepresentation = string.Empty;

            //noteDifference.ClosestNote = closestNote.Name; //noteNameRepresentation;

            //return noteDifference;
        }

        /// <summary>
        /// Finds the two closest open chord notes (as shown at NoteFrequencyFilter description) 
        /// of the played frequency.
        /// </summary>
        /// <param name="playedFrequency">The frequency the user played.</param>
        /// <returns>Returns the two closest open chord notes, 
        /// unless the playedFrequency is above the highest open string frequency, 
        /// or below the lowest open string frequency,
        /// in that case it will return the same value for both.</returns>
        private UpperAndLowerNotes GetTwoClosestOpenNotes(Hz playedFrequency)
        {
            UpperAndLowerNotes twoClosestOpenNotes = new UpperAndLowerNotes(Notes.LowerLimit, Notes.UpperLimit);  

            if (playedFrequency <= OpenStringNotes.E2.Hertz)
            {
                twoClosestOpenNotes.Lower = Notes.LowerLimit;
                twoClosestOpenNotes.Upper = OpenStringNotes.E2;
            }
            else if (playedFrequency > OpenStringNotes.E2.Hertz && playedFrequency <= OpenStringNotes.A2.Hertz)
            {
                twoClosestOpenNotes.Lower = OpenStringNotes.E2;
                twoClosestOpenNotes.Upper = OpenStringNotes.A2;
            }
            else if (playedFrequency > OpenStringNotes.A2.Hertz && playedFrequency <= OpenStringNotes.D3.Hertz)
            {
                twoClosestOpenNotes.Lower = OpenStringNotes.A2;
                twoClosestOpenNotes.Upper = OpenStringNotes.D3;
            }
            else if (playedFrequency > OpenStringNotes.D3.Hertz && playedFrequency <= OpenStringNotes.G3.Hertz)
            {
                twoClosestOpenNotes.Lower = OpenStringNotes.D3;
                twoClosestOpenNotes.Upper = OpenStringNotes.G3;
            }
            else if (playedFrequency > OpenStringNotes.G3.Hertz && playedFrequency <= OpenStringNotes.B3.Hertz)
            {
                twoClosestOpenNotes.Lower = OpenStringNotes.G3;
                twoClosestOpenNotes.Upper = OpenStringNotes.B3;
            }
            else if (playedFrequency > OpenStringNotes.B3.Hertz && playedFrequency <= OpenStringNotes.E4.Hertz)
            {
                twoClosestOpenNotes.Lower = OpenStringNotes.B3;
                twoClosestOpenNotes.Upper = OpenStringNotes.E4;
            }
            else if (playedFrequency > OpenStringNotes.E4.Hertz)
            {
                twoClosestOpenNotes.Lower = OpenStringNotes.E4;
                twoClosestOpenNotes.Upper = Notes.UpperLimit;
            }
        
            return twoClosestOpenNotes;
        }

        /// <summary>
        /// Find the single note which is closest to the frequency.
        /// </summary>
        /// <param name="twoClosestOpenNotes">The two open notes that the frequency is between them.</param>
        /// <param name="frequency">The given frequency.</param>
        /// <returns>Retruns the main note that is being tuned to.</returns>
        private Note FindClosestNote(UpperAndLowerNotes twoClosestOpenNotes, Hz frequency)
        {
            if ((twoClosestOpenNotes.Upper.Hertz - frequency) < (frequency - twoClosestOpenNotes.Lower.Hertz))
                return twoClosestOpenNotes.Upper;
            else
                return twoClosestOpenNotes.Lower;
        }

        /// <summary>
        ///  Gets the distance between the played frequency to the open note by percentage.
        /// </summary>
        /// <param name="twoClosestOpenNotes">The two closest open notes to the frequency.</param> 
        /// <param name="closestNote">The most closest note, out of the two closest notes, 
        /// which will be used as a reference to calculate the difference between this and the playedFrequency.</param>
        /// <param name="playedFrequency">The note that the user hits translated to frequency [Hz].</param>
        /// <returns>How close the users' frequency is from the desired note.</returns>
        private NoteDifference CalculateRatioOfCloseness(UpperAndLowerNotes twoClosestOpenNotes, Note closestNote, Hz playedFrequency)
        {
            float closenessToMark; //Closeness to the exact note mark. Number between 0 and 1.
            NoteDifference noteDiff = new NoteDifference();

            //Represents how close the frequency is from the closestNote.
            float difference = Math.Abs(playedFrequency - closestNote.Hertz); 
            
            float middleOfTwoOpenNotes = Math.Abs(twoClosestOpenNotes.Upper.Hertz - twoClosestOpenNotes.Lower.Hertz) / 2;

            if (middleOfTwoOpenNotes != 0)
                //Calculate the closness to the closestNote.
                closenessToMark = difference / middleOfTwoOpenNotes; //FIX: Why is that?
            else
                closenessToMark = difference;
            //Calculate the alpha (opacity) of the note indicator.
            //noteDiff.ClosnessAlpha = NoteDifference.FULL_ALPHA - closenessToMark;
            //Calculate the closness by a precentage of base 90.
            noteDiff.ClosnessByPercentage_Base90 = closenessToMark * 90; //FIX: This is not a precentage!
            noteDiff.ClosestNote = closestNote.Name;

            if (playedFrequency < closestNote.Hertz)
            {
                //from -90 to 0 degrees.
                noteDiff.ClosnessByPercentage_Base90 *= (-1);
                //if (closestNote.Alias != null)
                //    noteDiff.ClosestNote = closestNote.Alias;
            }

            return noteDiff;
        }
    }
}