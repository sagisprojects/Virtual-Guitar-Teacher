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
using System.Collections.Generic;
using static Virtual_Guitar_Teacher.Controller.Libraries.Generic;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    /// <summary>
    /// This class is responsible for playing the guitar intro animation,
    /// reading a song from a *.vgts file, animating a sequence of notes, 
    /// and comparing each note to the CurrentlyPlayedFrequency.
    /// </summary>
    public class NotesPlayer
    {
        const string SONG_FILE_EXTENSION = ".vgts";
        //protected Context _context;
        protected Activity _activity;

		public Hz CurrentlyPlayedFrequency { get; set; }

        //Events:
        public event EventHandler<EventArgs> OnIntroAnimationFinished;

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
            float target_ScaleX = 4.5f; //5;
            float target_ScaleY = 4.5f; //5;
            int target_ScrollX_2nd = 150; //135;

            int msDuration = 4000;
			int msDelay = 1000;

            guitarImage.Animate()
                .SetDuration(msDuration)
                .SetStartDelay(msDelay)
                .ScrollX(guitarImage, target_ScrollX)
                .ScrollY(guitarImage, target_ScrollY)
                .ScaleX(target_ScaleX)
                .ScaleY(target_ScaleY)
                .WithEndAction(new Runnable(() =>
                {
                    guitarImage.Animate()
                        .SetDuration(msDuration)
                        .SetStartDelay(msDelay)
                        .ScrollX(guitarImage, target_ScrollX_2nd)
                        .WithEndAction(new Runnable(() => {
                            OnIntroAnimationFinished(this, new EventArgs());
                        }))
                        .Start();
                }))
                .Start();
        }

        public void ShowStringsColors(TableLayout stringsColorsLegend)
        {
            const float FULL_ALPHA = 1;
            int msDuration = 1000;

            stringsColorsLegend.Animate()
                .SetDuration(msDuration)
                .Alpha(FULL_ALPHA);
        }

        public void HideStringsColors(TableLayout stringsColorsLegend)
        {
            const float TRANSPARENT = 0;
            int msDuration = 1000;

            stringsColorsLegend.Animate()
                .SetDuration(msDuration)
                .Alpha(TRANSPARENT);
        }

        /// <summary>
        /// Reads a sequence from a *.vgts file.
        /// </summary>
        /// <param name="fileName">The file's name (no extension).</param>
        /// <returns>Returns the sequence of that file.</returns>
        protected Sequence SequenceReader(string fileName)
        {
            //string songsFolderPath = _activity.Resources.GetString(Resource.String.SongsFolderPath);

            string[] sequenceLines = GetFileLinesFromAssets(fileName + SONG_FILE_EXTENSION, _activity.Assets);

            DiscardCommentsAndEmptyLines(ref sequenceLines);

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
        /// <returns>Returns a position of the currentNote which is relativly closest from previousNote.</returns>
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
                    else if ((stringsDistances[i - 1] == 0 && currentNote.Positions[i - 1].Fret == GuitarFret.OpenString)
                        || fretsDistances[i - 1] < fretsDistances[i])
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
        protected ObjectAnimator[] PrepareNotesAnimations(Sequence sequence)
        {
            ObjectAnimator[] animations = new ObjectAnimator[sequence.Length];

            for (int i = 0; i < sequence.Length; i++)
            {
                //Get note from sequence.
                Note note = sequence[i];
                //Create a new visual representation of that note.
                NoteRepresentation noteRep = new NoteRepresentation(_activity, note);
                //Create the note's animation, and add that animation to the animations array.
                animations[i] = noteRep.CreateNoteAnimation();
				//Sign up for sound input window events.
				noteRep.OnNoteArraival += NotesPlayer_OnNoteArraival;
				noteRep.OnNoteGone += NotesPlayer_OnNoteGone;
            }

            return animations;
        }
        
        public void SpotAnimateAllPositionsOfANote(Note note)
        {
            List<ObjectAnimator> animationsList = new List<ObjectAnimator>();
            foreach (Position position in note.Positions)
            {
                //Create a new visual representation of that note.
                NoteRepresentation noteRep = new NoteRepresentation(_activity, note);
                //Set name on note.
                noteRep.SetText(note.Name);
                //Create the note's animation, and add that animation to the animations array.
                animationsList.Add(noteRep.CreateNoteAnimation_FadeOnly(position));
            }
            AnimatorSet animatorSet = new AnimatorSet();
            animatorSet.PlayTogether(animationsList.ToArray());
            animatorSet.Start();
        }

        private void NotesPlayer_OnNoteArraival(object sender, NoteRepresentation.OnNoteArraivalArgs songNote)
		{
			bool isOnPoint = CompareNotes(CurrentlyPlayedFrequency, songNote.Frequency);

			if (isOnPoint) 
			{
				//Change circle to a smiley face.
				((NoteRepresentation)sender).ChangeToSmileyFace(true);
			}
		}

		private void NotesPlayer_OnNoteGone(object sender, EventArgs e)
		{
            //throw new NotImplementedException();
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

            //animatorSet.AnimationEnd
            //animatorSet.Pause
        }
    }
}