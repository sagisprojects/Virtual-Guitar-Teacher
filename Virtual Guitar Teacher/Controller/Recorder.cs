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
using Virtual_Guitar_Teacher.Controller.Libraries;

namespace Virtual_Guitar_Teacher.Controller
{
    class Recorder : NotesPlayer
    {
        //Record input from microphone.
        //Record a sequence from screen to file.

        Note[] _notesArray;

        public Recorder(Activity activity)
            : base(activity)
        {
            _notesArray = new Notes().ToArray();
        }

        public Note FindClosestNote(Hz frequency)
        {
            float currentDifference, prevDifference = Notes.UpperLimit.Hertz;
            Note tempNote = null;
            foreach (Note note in _notesArray)
            {
                currentDifference = Math.Abs(note.Hertz - frequency);
                if (currentDifference < prevDifference)
                    tempNote = note;
                else
                    break;
                prevDifference = currentDifference;
            }
            return tempNote;
        }
    }
}