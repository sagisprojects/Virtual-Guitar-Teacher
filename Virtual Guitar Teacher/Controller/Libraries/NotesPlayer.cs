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
using Android.Content.Res;
using System.Reflection;
using Runnable = Java.Lang.Runnable;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    /// <summary>
    /// Allows the following:
    /// * Sending the notes to its destination on the screen.
    /// * Copmering between notes.
    /// </summary>
    public class NotesPlayer
    {
        const string SONG_FILE_EXTENSION = ".vgts";
        //protected Context _context;
        protected Activity _activity;

        //Events:
        public event EventHandler<EventArgs> OnIntroAnimationFinished;
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
            int target_ScrollY = -5;
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
                .WithEndAction(new Runnable(() =>
                {
                    guitarImage.Animate()
                        .SetDuration(msDuration)
                        .SetStartDelay(initialDelay)
                        .ScrollX(guitarImage, target_ScrollX_2nd)
                        .WithEndAction(new Runnable(() => {
                            OnIntroAnimationFinished(this, new EventArgs());
                        }))
                        .Start();
                }))
                .Start();

            /*guitarImage.Animate()
                .SetDuration(msDuration)
                .SetStartDelay(initialDelay * 2 + msDuration)
                .ScrollX(guitarImage, target_ScrollX_2nd);*/
        }

        // <summary>
        // Represents a sequence of notes.
        // </summary>

        //Name  Delay Duration
        //G3    0.5    1
        //A2    0.5    1
        //C3    0.5    1
        //E4    2.0    1
        //F4    0.5    1
        //E3    0.5    1
        //C3    0.5    1

        
        /// <summary>
        /// Reads a sequence from a *.vgts file.
        /// </summary>
        /// <param name="fileName">The file's name (no extension).</param>
        /// <returns>Returns the sequence of that file.</returns>
        protected Sequence SequenceReader(string fileName)
        {
            string songsFolderPath = _activity.Resources.GetString(Resource.String.SongsFolderPath);

            string[] sequenceLines = Generic.GetFileLinesFromAssets(fileName + SONG_FILE_EXTENSION, _activity.Assets);
            
            //string[] sequenceLines = File.ReadAllLines(songsFolderPath + fileName);
            Sequence sequence = new Sequence(sequenceLines.Length);
            string[] lineSegments;
            Notes notesReference = new Notes();

            for (int i = 0; i < sequenceLines.Length; i++)
            {
                string line = sequenceLines[i];
                lineSegments = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                NoteName noteName;
                double delay, duration;

                bool isNoteName = NoteName.TryParse(lineSegments[0], out noteName);
                bool isDouble1 = double.TryParse(lineSegments[1], out delay);
                bool isDouble2 = double.TryParse(lineSegments[2], out duration);
                
                if (!(isNoteName && isDouble1 && isDouble2))
                    throw new Exception("Parsing segments of song file failed. "
                        + "Only the following format is allowed: string double double. "
                        + "And make sure the string name is spelled correctly.");

                Note note = notesReference[noteName];

                if (i == 0)
                    note = new Note(noteName,
                    note.Positions[0],
                    delay,
                    duration);
                else
                    note = new Note(noteName,
                    CalculateClosestPositionFromPreviousNote(sequence[i - 1], note),
                    delay,
                    duration);

                sequence[i] = note;
            }
            return sequence;
        }

        /// <summary>
        /// Get the position which has the
        /// closest fret, unless it's on the same string and it's open.
        /// </summary>
        /// <param name="previousNote">The note from which to measure.</param>
        /// <param name="currentNote">The note which its position needs to be calculated.</param>
        /// <returns></returns>
        private Position CalculateClosestPositionFromPreviousNote(Note previousNote, Note currentNote)
        {
            if (currentNote.Positions.Length == 1)
                return currentNote.Positions[0];

            int[] fretsDistances = new int[currentNote.Positions.Length];
            int[] stringsDistances = new int[currentNote.Positions.Length];
            Position closestPosition = null;

            for (int i = 0; i < currentNote.Positions.Length; i++)
            {
                fretsDistances[i] = Math.Abs(previousNote.Position.Fret - currentNote.Positions[i].Fret);
                stringsDistances[i] = Math.Abs(previousNote.Position.Fret - currentNote.Positions[i].Fret);

                if (i > 0)
                {
                    if (stringsDistances[i] == 0 && currentNote.Positions[i].Fret == GuitarFret.OpenString)
                        closestPosition = currentNote.Positions[i];
                    else if (stringsDistances[i - 1] == 0 && currentNote.Positions[i - 1].Fret == GuitarFret.OpenString)
                        closestPosition = currentNote.Positions[i - 1];
                    else if (fretsDistances[i - 1] < fretsDistances[i])
                        closestPosition = currentNote.Positions[i - 1];
                    else
                        closestPosition = currentNote.Positions[i];
                }
            }

            return closestPosition;
        }

        /// <summary>
        /// Converts a sequence to an array of animations.
        /// </summary>
        /// <param name="sequence">The sequence from which to gather animations information.</param>
        /// <returns>Returns a series of animations.</returns>
        protected ObjectAnimator[] PrepareNoteAnimations(Sequence sequence)
        {
            //PropertyViewAnimator[] animations = new PropertyViewAnimator[sequence.Length];
            ObjectAnimator[] animations = new ObjectAnimator[sequence.Length];

            for (int i = 0; i < sequence.Length; i++)
            {
                //Get note from sequence.
                Note note = sequence[i];
                //Create a new visual representation of that note.
                NoteRepresentation noteRep = new NoteRepresentation(_activity, note);
                //Create the note's animation, and add that animation to the animations array.
                animations[i] = noteRep.CreateNoteAnimation();
            }

            return animations;
        }

        /// <summary>
        /// Playes a series of animations.
        /// </summary>
        /// <param name="animations">The series of animations to be played.</param>
        protected void PlayAnimations(ObjectAnimator[] animations)
        {
            AnimatorSet animatorSet = new AnimatorSet();
            animatorSet.PlaySequentially(animations);
            animatorSet.Start();
        }
    }
}