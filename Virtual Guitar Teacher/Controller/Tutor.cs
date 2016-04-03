using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Virtual_Guitar_Teacher.Controller;
using Virtual_Guitar_Teacher.Controller.Libraries;
using Android.Util;

namespace Virtual_Guitar_Teacher.Controller
{
    class Tutor : NotesPlayer
    {
        //private Context _context;
        private Activity _activity;

        //Intialization
        public Tutor(Activity activity)
            : base(activity)
        {
            _activity = activity;
        }

        public void StartTutoring()
        {
            NoteRepresentation noteRep = new NoteRepresentation(_activity, 
                BallColor.Blue, GuitarString.G, new Note(NotesNames.G3));
            Log.Info("Tutor", "StartTutoring");
            noteRep.AnimateNote();
        }

        //base.SendNote

        //base.CompareNotes

        //FFT
    }
}