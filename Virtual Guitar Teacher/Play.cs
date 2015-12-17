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

namespace Virtual_Guitar_Teacher
{
    class Play : NotesPlayer
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

        //Initialize Song Play

        //PlaySong - Creates a new MediaPlayer class,
        //              uses the MediaPlayer to play the chosen song from the list of songs.

        //base.SendNote

        //base.CompareNotes

        //base.FFT
    }
}