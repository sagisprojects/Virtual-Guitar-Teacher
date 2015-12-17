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
    class Tools
    {
    }


    public struct OpenStringNotes
    {
        public const double E4 = 329.63;
        public const double B3 = 246.94;
        public const double G3 = 196.00;
        public const double D3 = 146.83;
        public const double A2 = 110.00;
        public const double E2 = 82.41;
        //TODO: Add all the rest of the frequencies/notes.
    }
}