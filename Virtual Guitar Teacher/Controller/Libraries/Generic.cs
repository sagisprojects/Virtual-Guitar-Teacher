using System;
using Android.App;
using Android.Util;
using System.Reflection;
using Android.Content.Res;
using System.IO;
using System.Collections.Generic;

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

        public static string[] GetFileLinesFromAssets(string fileName, AssetManager assets)
        {
            Stream stream = assets.Open(fileName);
            StreamReader streamReader = new StreamReader(stream);
            List<string> fileLines = new List<string>();

            while (!streamReader.EndOfStream)
            {
                fileLines.Add(streamReader.ReadLine());
            }

            return fileLines.ToArray();
        }

        /// <summary>
        /// Discards any commented lines (//) or empty lines.
        /// </summary>
        /// <param name="lines">The array of the stringed lines from which to remove commented or empty line.</param>
        public static void DiscardCommentsOrEmptyLines(ref string[] lines)
        {
            List<string> linesList = new List<string>();
            foreach (string line in lines)
            {
                if (line[0] == '/' && line[1] == '/'
                    || string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                    continue;
                linesList.Add(line);
            }
            lines = linesList.ToArray();
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
        public GuitarString String { get; set; }
        public GuitarFret Fret { get; set; }

        public Position(GuitarString stringNum, GuitarFret fretNum)
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
            Ds2 = "Ds2", Eb2 = "Eb2",
            E2 = "E2",
            F2 = "F2",
            Fs2 = "Fs2", Gb2 = "Gb2",
            G2 = "G2",
            Gs2 = "Gs2", Ab2 = "Ab2",
            A2 = "A2",
            As2 = "As2", Bb2 = "Bb2",
            B2 = "B2",

            C3 = "C3",
            Cs3 = "Cs3", Db3 = "Db3",
            D3 = "D3",
            Ds3 = "Ds3", Eb3 = "Eb3",
            E3 = "E3",
            F3 = "F3",
            Fs3 = "Fs3", Gb3 = "Gb3",
            G3 = "G3",
            Gs3 = "Gs3", Ab3 = "Ab3",
            A3 = "A3",
            As3 = "As3", Bb3 = "Bb3",
            B3 = "B3",

            C4 = "C4",
            Cs4 = "Cs4", Db4 = "Db4",
            D4 = "D4",
            Ds4 = "Ds4", Eb4 = "Eb4",
            E4 = "E4",
            F4 = "F4",
            Fs4 = "Fs4", Gb4 = "Gb4",
            G4 = "G4",
            Gs4 = "Gs4", Ab4 = "Ab4",
            A4 = "A4",
            As4 = "As4", Bb4 = "Bb4";
            /*B4 = "B4",

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
            B5 = "B5";*/
    }

    public class FretMetrics
    {
        private int _x, _y, _width, _height;

        public FretMetrics(int x, int y, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public int GetHorizontalCenter()
        {
            return _x + (_width / 2);
        }

        public int GetVerticalCenter()
        {
            return _y + (_height / 2);
        }

        public int GetTop()
        {
            return _y;
        }

        public int GetBottom()
        {
            return _y + _height;
        }
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
                Logger.Log(ex);
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
    /// Defines the frets on the guitar as view object resources.
    /// </summary>
    public enum GuitarFret
    {
        OpenString = 0,
        Fret1 = 1,
        Fret2 = 2,
        Fret3 = 3,
        Fret4 = 4,
        Fret5 = 5,
        Fret6 = 6
    }

    /// <summary>
    /// Defines set colors for notes representation.
    /// </summary>
    public enum BallColor
    {
        Red = 0,
        Blue = 1,
        Blue2 = 2,
        Green = 3,
        Green2 = 4,
        Orange = 5,
        Purple = 6
    }
}