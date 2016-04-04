using System;
using Android.App;
using Android.Util;
using System.Reflection;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    /// <summary>
    /// A generic class for generic methods.
    /// </summary>
    public static class Generic
    {
        /// <summary>
        /// Gets the activity's display measurments.
        /// </summary>
        /// <param name="activity">The reference activity from which to gather the display info.</param>
        /// <returns>Returns the display's measurments.</returns>
        public static DisplayMetrics GetScreenDimentions(Activity activity)
        {
            DisplayMetrics displayMetrics = new DisplayMetrics();
            activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            return displayMetrics;
        }
    }

    /// <summary>
    /// Defines a sequence of notes.
    /// </summary>
    public class Sequence
    {
        private Note[] _notesArray;
        private int _length;

        public int Length { get { return _length; } }

        public Sequence(int arraySize)
        {
            _notesArray = new Note[arraySize];
            _length = arraySize;
        }

        public Note this[int index]
        {
            get { return _notesArray[index]; }
            set { _notesArray[index] = value; }
        }
    }

    /// <summary>
    /// Defines a position on the guitar's neck, by a string number and a fret number.
    /// </summary>
    public class Position
    {
        public int String { get; set; }
        public int Fret { get; set; }

        public Position(int stringNum, int fretNum)
        {
            String = stringNum;
            Fret = fretNum;
        }
    }

    /// <summary>
    /// Defines the absolote names for each of the notes from Eb2 to B5.
    /// </summary>
    public static class NotesNames
    {
        public static string 
            Eb2 = "Eb2",
            E2 = "E2",
            F2 = "F2",
            Fs2 = "Fs2",
            G2 = "G2",
            Ab2 = "Ab2",
            A2 = "A2",
            Bb2 = "Bb2",
            B2 = "B2",

            C3 = "C3",
            Cs3 = "Cs3",
            D3 = "D3",
            Eb3 = "Eb3",
            E3 = "E3",
            F3 = "F3",
            Fs3 = "Fs3",
            G3 = "G3",
            Ab3 = "Ab3",
            A3 = "A3",
            Bb3 = "Bb3",
            B3 = "B3",

            C4 = "C4",
            Cs4 = "Cs4",
            D4 = "D4",
            Eb4 = "Eb4",
            E4 = "E4",
            F4 = "F4",
            Fs4 = "Fs4",
            G4 = "G4",
            Ab4 = "Ab4",
            A4 = "A4",
            Bb4 = "Bb4",
            B4 = "B4",

            C5 = "C5",
            Cs5 = "Cs5",
            D5 = "D5",
            Eb5 = "Eb5",
            E5 = "E5",
            F5 = "F5",
            Fs5 = "Fs5",
            G5 = "G5",
            Ab5 = "Ab5",
            A5 = "A5",
            Bb5 = "Bb5",
            B5 = "B5";
    }

    /// <summary>
    /// Defines a named note.
    /// The name of the note must correspond to the ones contained in NotesNames.
    /// </summary>
    public struct NoteName
    {
        private string _value;

        public NoteName(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
            set
            {
                FieldInfo[] fields = typeof(NotesNames).GetFields();
                foreach (var field in fields)
                {
                    if (field.ToString() == value)
                        _value = value;
                    else
                        throw new Exception("Invalid note name: Note must be one of the NotesNames.");
                }
            }
        }

        static public implicit operator NoteName(string value)
        {
            return new NoteName(value);
        }

        static public implicit operator string(NoteName value)
        {
            return value.Value;
        }

        static public bool TryParse(string s, out NoteName result)
        {
            try
            {
                result = new NoteName(s);
                return true;
            }
            catch (Exception ex)
            {
                result = null;
                return false;
            }
        }
    }

    /// <summary>
    /// Defines the unit of measurment for a frequency.
    /// </summary>
    public struct Hz
    {
        public const short TIME_IN_SECONDS = 1;
        private float _cyclesPerSecond;
        public float CyclesPerSecond
        {
            get { return _cyclesPerSecond; }
            set { _cyclesPerSecond = value; }
        }
        /*public short TimeInSeconds
        {
            get { return _timeInSeconds; }
            set { _timeInSeconds = value; }
        }*/
    }

    /// <summary>
    /// Orgenizes the order of the strings on the guitar.
    /// </summary>
    public enum GuitarString
    {
        e = 1,
        B = 2,
        G = 3,
        D = 4,
        A = 5,
        E = 6
    }

    /// <summary>
    /// Defines set colors for notes representation.
    /// </summary>
    public enum BallColor
    {
        Blue, Green, Orange, Purple, Red
    }
}