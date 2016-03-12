using System;
using Virtual_Guitar_Teacher.Controller.Libraries;

namespace Virtual_Guitar_Teacher.Controller
{
    class Player : NotesPlayer
    {
        public Player()
        {
            OnNoteArraival += Player_OnNoteArraival;
        }

        private void Player_OnNoteArraival(object sender, OnNoteArraivalArgs e)
        {
            throw new NotImplementedException();
        }

        //Initialize Song Play

        //PlaySong - Creates a new MediaPlayer class,
        //              uses the MediaPlayer to play the chosen song from the list of songs.

        //base.SendNote

        //base.CompareNotes

        //base.FFT
    }
}