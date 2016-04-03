using Android.Animation;
using Android.App;
using Android.Util;
using Android.Views.Animations;
using Android.Widget;
using System;
using System.IO;
using System.Threading;
using Android.Content;
using Android.Graphics;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    /// <summary>
    /// Allows the following:
    /// * Sending the notes to its destination on the screen.
    /// * Copmering between notes.
    /// </summary>
    public class NotesPlayer
    {
        const long ONE_MILISECOND = 1000;

        //protected Context _context;
        protected Activity _activity;

        //Events:
        //OnNoteArraival - once the note arrives to it's destination line.
        public event EventHandler<OnNoteArraivalArgs> OnNoteArraival;
        // Special EventArgs class to hold info about...
        public class OnNoteArraivalArgs : EventArgs
        {
            private double newArea;

            public OnNoteArraivalArgs(double d)
            {
                newArea = d;
            }
            public double NewArea
            {
                get { return newArea; }
            }
        }

        public NotesPlayer(Activity activity)
        {
            //_context = context;
            _activity = activity;
        }

        /// <summary>
        /// Compares between the mic input frequency and the song note.
        /// </summary>
        /// <param name="userNote">The played frequency.</param>
        /// <param name="songNote">The note that is given by the song.</param>
        /// <returns>Returns true if the input frequency matches the song's frequency.</returns>
        protected bool CompareNotes(float userNote, float songNote)
        {
            const float ONE_AND_A_HALF_PERCENT = 1.015f; //TODO: That's not one and a half percent!?
            bool isMatching = false;
            //Find if the userNote is in the range between:
            //1.5% out of songNote below songNote and 1.5% out of songNote above songNote.
            if (userNote < songNote * ONE_AND_A_HALF_PERCENT 
                && userNote > songNote / ONE_AND_A_HALF_PERCENT)
                isMatching = true;
            
            return isMatching;
        }

        /// <summary>
        /// Animates the guitar intro which should last 10 seconds.
        /// </summary>
        /// <param name="guitarImage">The object which contains the image of the guitar.</param>
        public void AnimateGuitarIntro(ImageView guitarImage)
        {
            int target_ScrollX = 230;
            int target_ScrollY = -7;
            float target_ScaleX = 5;
            float target_ScaleY = 5;
            int target_ScrollX_2nd = 135;

            int msDuration = 4000;
            int initialDelay = 1000;

            guitarImage.Animate()
                .SetDuration(msDuration)
                .SetStartDelay(initialDelay)
                .ScrollX(guitarImage, target_ScrollX)
                .ScrollY(guitarImage, target_ScrollY)
                .ScaleX(target_ScaleX)
                .ScaleY(target_ScaleY)
                .Start();

            guitarImage.Animate()
                .SetDuration(msDuration)
                .SetStartDelay(initialDelay * 2 + msDuration)
                .ScrollX(guitarImage, target_ScrollX_2nd);
        }

        /// <summary>
        /// Represents a sequence of notes.
        /// </summary>

        //Name String# Fret# Delay Duration
        //G3     3       0    0.5    1
        //A2     5       0    0.5    1
        //C3     3       5    0.5    1
        //E4     1       0    2.0    1
        //F4     1       1    0.5    1
        //E3     4       2    0.5    1
        //C3     2       1    0.5    1

        //SequenceReader
        protected Sequence SequenceReader(string fileName)
        {
            string songsFolderPath = @"\VGT\Documents\Songs\"; //TODO: This should be referenced from Strings.xml.

            string[] sequenceLines = File.ReadAllLines(songsFolderPath + fileName);
            Sequence sequence = new Sequence(sequenceLines.Length);
            string[] lineSegments;

            for (int i = 0; i < sequenceLines.Length; i++)
            {
                string line = sequenceLines[i];
                lineSegments = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                NoteName noteName;
                int stringNum, fretNum;
                double delay, duration;

                bool isNoteName = NoteName.TryParse(lineSegments[0], out noteName);
                bool isInt1 = int.TryParse(lineSegments[1], out stringNum);
                bool isInt2 = int.TryParse(lineSegments[2], out fretNum);
                bool isDouble1 = double.TryParse(lineSegments[3], out delay);
                bool isDouble2 = double.TryParse(lineSegments[4], out duration);

                if (!(isNoteName && isInt1 && isInt2 && isDouble1 && isDouble2))
                    throw new Exception("Parsing segments of song file failed. "
                        + "Only the following format is allowed: string int int double double. "
                        + "And make sure the string name is spelled correctly.");

                sequence[i] = new Note(noteName,
                    new Notes()[noteName].Hertz,
                    new Position(stringNum, fretNum),
                    delay,
                    duration);
            }
            return sequence;
        }

        //PlaySequence
        protected void PlaySequence(Sequence sequence)
        {
            for (int i = 0; i < sequence.Length; i++)
            {
                Note note = sequence[i];
                NoteRepresentation noteRep = new NoteRepresentation(_activity,
                    (BallColor)note.Position.Fret, 
                    (GuitarString)note.Position.String, 
                    note);
                noteRep.AnimateNote(
                    (long)(note.Duration * ONE_MILISECOND),
                    (long)(note.Delay * ONE_MILISECOND));
            }
        }
    }
}