using Android.App;
using System;
using Virtual_Guitar_Teacher.Controller.Libraries;

namespace Virtual_Guitar_Teacher.Controller
{
    class Player : NotesPlayer
    {
        //private Activity _activity;

        public Player(Activity activity)
            : base(activity)
        {
            //_activity = activity;
        }
    }
}