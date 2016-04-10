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
using Android.Animation;

namespace Virtual_Guitar_Teacher.Controller
{
    class Tutor : NotesPlayer
    {
        //private Context _context;
        //private Activity _activity;
        private Sequence _sequence;

        //Intialization
        public Tutor(Activity activity)
            : base(activity)
        {
            //_activity = activity;
        }

        public void ReadSong()
        {
            /*NoteRepresentation noteRep = new NoteRepresentation(_activity, 
                BallColor.Blue, GuitarString.G, new Note(NotesNames.G3));
            Log.Info("Tutor", "StartTutoring");
            noteRep.AnimateNote();*/
            //TODO: Create the song file.
            string fileName = "TestSong"; //"TinyJonathan";
            _sequence = SequenceReader(fileName);
        }

        /// <summary>
        /// Plays the selected song.
        /// </summary>
        public void PlaySong()
        {
            ObjectAnimator[] animations = null;
            if (_sequence != null)
                animations = PrepareNoteAnimations(_sequence);
            if (animations != null)
                PlayAnimations(animations);
        }

        //base.SendNote

        //base.CompareNotes

        //FFT
    }
}