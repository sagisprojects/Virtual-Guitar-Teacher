using System;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    /// <summary>
    /// Allows the following:
    /// * Sending the notes to its destination on the screen.
    /// * Copmering between notes.
    /// </summary>
    class NotesPlayer
    {
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

        /// <summary>
        /// Compares between the mic input frequency and the song note.
        /// </summary>
        /// <param name="userNote">The played frequency.</param>
        /// <param name="songNote">The note that is given by the song.</param>
        /// <returns>Returns true if the input frequency matches the song's frequency.</returns>
        protected bool CompareNotes(float userNote, float songNote)
        {
            const float ONE_AND_A_HALF_PERCENT = 1.015f;
            bool isMatching = false;
            //Find if the userNote is in the range between:
            //1.5% out of songNote below songNote and 1.5% out of songNote above songNote.
            if (userNote < songNote * ONE_AND_A_HALF_PERCENT 
                && userNote > songNote / ONE_AND_A_HALF_PERCENT)
                isMatching = true;
            
            return isMatching;
        }

        //Send Note - Moves the visual indication of a note to the destination line.
    }
}