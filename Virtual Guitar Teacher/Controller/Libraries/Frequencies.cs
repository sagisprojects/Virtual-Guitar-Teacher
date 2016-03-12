using System;
using System.Collections.Generic;
using System.Drawing;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    /// <summary>
    /// Defines all of the relevant notes which can be played on a guitar, as frequencies.
    /// </summary>
    /*static class Frequencies
    {*/
        public class Note
        {
            private string _name;
            private Hz _hertz;
            private Point[] _positions;
            public string Name
            {
                get { return _name; }
            }
            public Hz Hertz
            {
                get { return _hertz; }
            }
            public Point[] Positions
            {
                get { return _positions; }
            }
            public Note(string name, Hz hertz, Point[] positions)
            {
                _name = name;
                _hertz = hertz;
                _positions = positions;
            }
        }
    
        /// <summary>
        /// Legend:
        /// Xs   = X-sharp (half a note higher), musical representation: X♯.
        /// Xb   = X-bemol (half a note lower), musical representation: X♭.
        /// X0   = The number after the note's latter indicats which octave this note is on.
        /// 
        /// Base Frequency:
        /// A4 = 440 Hz.
        /// 
        /// For Piano Players:
        /// Middle C is in octave number 4:
        /// C4 = 261.63 (±0)
        /// 
        /// For Guitar Players:
        /// E2 = 82.407      (Bass)
        /// A2 = 110.000     (Bass)
        /// D3 = 146.832     (Bass)
        /// G3 = 195.998     (Naylon)
        /// B3 = 246.942     (Naylon)
        /// E4 = 329.628     (Naylon)
        /// </summary>
        public class Notes
        {
            private static Note[] _notes;

            public Note this[string name]
            {
                get { return Array.Find(_notes, x => x.Name == name); }
                /*set
                {
                    int index = Array.FindIndex(_notes, x => x.Name == name);
                    _notes[index] = value;
                }*/
            }

            static Notes()
            {
                _notes = new Note[]
                {
                    new Note("Eb2",
                    new Hz() { CyclePerSecond = 77.782f },
                    null),
                    new Note("E2",
                    new Hz() { CyclePerSecond = 82.407f },                          //Open string number 6 (Lowest note of guitar) E2.
                    null),
                    new Note("F2",
                    new Hz() { CyclePerSecond = 87.307f },
                    new Point[] {new Point(0,0)}),
                    new Note("Fs2",
                    new Hz() { CyclePerSecond = 92.499f },
                    new Point[] {new Point(0,1)}),
                    new Note("G2",
                    new Hz() { CyclePerSecond = 97.999f },
                    new Point[] {new Point(0,2)}),
                    new Note("Ab2",
                    new Hz() { CyclePerSecond = 103.826f },
                    new Point[] {new Point(0,3)}),
                    new Note("A2",
                    new Hz() { CyclePerSecond = 110.000f },                         //Open string number 5 - A2.
                    new Point[] {new Point(0,4)}),
                    new Note("Bb2",
                    new Hz() { CyclePerSecond = 116.541f },
                    new Point[] {new Point(0,5), new Point(1,0)}),
                    new Note("B2",
                    new Hz() { CyclePerSecond = 123.471f },
                    new Point[] {new Point(0,6), new Point(1,1)}),
                    new Note("C3",
                    new Hz() { CyclePerSecond = 130.813f },
                    new Point[] {new Point(0,7), new Point(1,2)}),
                    new Note("Cs3",
                    new Hz() { CyclePerSecond = 138.591f },
                    new Point[] {new Point(0,8), new Point(1,3)}),
                    new Note("D3",
                    new Hz() { CyclePerSecond = 146.832f },                         //Open string number 4 - D3.
                    new Point[] {new Point(0,9), new Point(1,4)}),
                    new Note("Eb3",
                    new Hz() { CyclePerSecond = 155.563f },
                    new Point[] {new Point(0,10), new Point(1,5), new Point(2,0)}),
                    new Note("E3",
                    new Hz() { CyclePerSecond = 164.814f },
                    new Point[] {new Point(0,11), new Point(1,6), new Point(2,1)}),
                    new Note("F3",
                    new Hz() { CyclePerSecond = 174.614f },
                    new Point[] {new Point(0,12), new Point(1,7), new Point(2,2)}),
                    new Note("Fs3",
                    new Hz() { CyclePerSecond = 184.997f },
                    new Point[] {new Point(0,13), new Point(1,8), new Point(2,3)}),
                    new Note("G3",
                    new Hz() { CyclePerSecond = 184.997f },                         //Open string number 3 - G3.
                    new Point[] {new Point(0,14), new Point(1,9), new Point(2,4)}),
                    new Note("Ab3",
                    new Hz() { CyclePerSecond = 207.652f },
                    new Point[] {new Point(1,10), new Point(2,5), new Point(3,0)}),
                    new Note("A3",
                    new Hz() { CyclePerSecond = 220.000f },
                    new Point[] {new Point(1,11), new Point(2,6), new Point(3,1)}),
                    new Note("Bb3",
                    new Hz() { CyclePerSecond = 233.082f },
                    new Point[] {new Point(1,12), new Point(2,7), new Point(3,2)}),
                    new Note("B3",
                    new Hz() { CyclePerSecond = 246.942f },                         //Open string number 2 - B3
                    new Point[] {new Point(1,13), new Point(2,8), new Point(3,3)}),
                    new Note("C4",
                    new Hz() { CyclePerSecond = 261.626f },
                    new Point[] {new Point(1,14), new Point(2,9), new Point(3,4), new Point(4,0)}),
                    new Note("Cs4",
                    new Hz() { CyclePerSecond = 277.183f },
                    new Point[] {new Point(2,10), new Point(3,5), new Point(4,1)}),
                    new Note("D4",
                    new Hz() { CyclePerSecond = 293.665f },
                    new Point[] {new Point(2,11), new Point(3,6), new Point(4,2)}),
                    new Note("Eb4",
                    new Hz() { CyclePerSecond = 311.127f },
                    new Point[] {new Point(2,12), new Point(3,7), new Point(4,3)}),
                    new Note("E4",
                    new Hz() { CyclePerSecond = 329.628f },                         //Open string number 1 - E4.
                    new Point[] {new Point(2,13), new Point(3,8), new Point(4,4)}),
                    new Note("F4",
                    new Hz() { CyclePerSecond = 349.228f },
                    new Point[] {new Point(2,14), new Point(3,9), new Point(4,5), new Point(5,0)}),
                    new Note("Fs4",
                    new Hz() { CyclePerSecond = 369.994f },
                    new Point[] {new Point(3,10), new Point(4,6), new Point(5,1)}),
                    new Note("G4",
                    new Hz() { CyclePerSecond = 391.995f },
                    new Point[] {new Point(3,11), new Point(4,7), new Point(5,2)}),
                    new Note("Ab4",
                    new Hz() { CyclePerSecond = 415.305f },
                    new Point[] {new Point(3,12), new Point(4,8), new Point(5,3)}),
                    new Note("A4",
                    new Hz() { CyclePerSecond = 440.000f },
                    new Point[] {new Point(3,13), new Point(4,9), new Point(5,4)}),
                    new Note("Bb4",
                    new Hz() { CyclePerSecond = 466.164f },
                    new Point[] {new Point(3,14), new Point(4,10), new Point(5,5)}),
                    new Note("B4",
                    new Hz() { CyclePerSecond = 493.883f },
                    new Point[] {new Point(4,11), new Point(5,6)}),
                    new Note("C5",
                    new Hz() { CyclePerSecond = 523.251f },
                    new Point[] {new Point(4,12), new Point(5,7)}),
                    new Note("Cs5",
                    new Hz() { CyclePerSecond = 554.365f },
                    new Point[] {new Point(4,13), new Point(5,8)}),
                    new Note("D5",
                    new Hz() { CyclePerSecond = 587.330f },
                    new Point[] {new Point(4,14), new Point(5,9)}),
                    new Note("Eb5",
                    new Hz() { CyclePerSecond = 622.254f },
                    new Point[] {new Point(5,10)}),
                    new Note("E5",
                    new Hz() { CyclePerSecond = 659.255f },
                    new Point[] {new Point(5,11)}),
                    new Note("F5",
                    new Hz() { CyclePerSecond = 698.456f },
                    new Point[] {new Point(5,12)}),
                    new Note("Fs5",
                    new Hz() { CyclePerSecond = 739.989f },
                    new Point[] {new Point(5,13)}),
                    new Note("G5",
                    new Hz() { CyclePerSecond = 783.991f },
                    new Point[] {new Point(5,14)}),
                    new Note("Ab5",
                    new Hz() { CyclePerSecond = 830.609f },
                    new Point[] {new Point(5,15)}),
                    new Note("A5",
                    new Hz() { CyclePerSecond = 880.000f },
                    new Point[] {new Point(5,16)}),
                    new Note("Bb5",
                    new Hz() { CyclePerSecond = 932.328f },
                    new Point[] {new Point(5,17)}),
                    new Note("B5",
                    new Hz() { CyclePerSecond = 987.767f },
                    new Point[] {new Point(5,18)}),
                };
            }
        }

    /// <summary>
    /// Defines the six guitar open strings' frequencies in a single array.
    /// </summary>
    /*public static class OpenStrings
    {
        public static Note[] _notes; //An array of open strings
            
        static OpenStrings()
        {
            Notes notes = new Notes();
            _notes = new Note[6];
            _notes[0] = notes["E2"];
            _notes[1] = notes["A2"];
            _notes[2] = notes["D3"];
            _notes[3] = notes["G3"];
            _notes[4] = notes["B3"];
            _notes[5] = notes["E4"];
        }
    }*/
    /// <summary>
    /// Defines the six guitar open strings' frequencies in a single array.
    /// </summary>
    public struct OpenStrings
    {
        public const float
            E4 = 329.628f,      //Nylon
            B3 = 246.942f,      //Nylon
            G3 = 195.998f,      //Nylon
            D3 = 146.832f,      //Bass
            A2 = 110.000f,      //Bass
            E2 = 82.407f;       //Bass
    }
    public struct Hz
    {
        public const short TIME_IN_SECONDS = 1;
        private float _cyclePerSecond;
        public float CyclePerSecond
        {
            get { return _cyclePerSecond; }
            set { _cyclePerSecond = value; }
        }
        /*public short TimeInSeconds
        {
            get { return _timeInSeconds; }
            set { _timeInSeconds = value; }
        }*/
    }
}
