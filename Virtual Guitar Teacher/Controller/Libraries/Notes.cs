using System;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    /// <summary>
    /// Represents a single note.
    /// </summary>
    public class Note
    {
        private NoteName _name;
        private Hz _hertz;
        private Position _position;
        private double _delay;
        private double _duration;
        public NoteName Name
        {
            get { return _name; }
        }
        public Hz Hertz
        {
            get { return _hertz; }
        }
        public Position Position
        {
            get { return _position; }
        }
        public double Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }
        public double Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        public Note(NoteName name)
        {
            Note note = new Notes()[name];
            _name = note.Name;
            _hertz = note.Hertz;
        }
        public Note(NoteName name, Hz hertz)
        {
            _name = name;
            _hertz = hertz;
        }
        public Note(NoteName name, Hz hertz, Position positions, double delay, double duration)
        {
            _name = name;
            _hertz = hertz;
            _position = positions;
            _delay = delay;
            _duration = duration;
        }
    }
    /// <summary>
    /// Defines all of the relevant notes which can be played on a guitar, as frequencies.
    /// 
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

            public Note this[NoteName name]
            {
                get { return Array.Find(_notes, x => x.Name.ToString() == name.ToString()); }
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
                    new Note(NotesNames.Eb2,
                    new Hz() { CyclesPerSecond = 77.782f }),
                    new Note(NotesNames.E2,
                    new Hz() { CyclesPerSecond = 82.407f }),                          //Open string number 6 (Lowest note of guitar) E2.
                    new Note(NotesNames.F2,
                    new Hz() { CyclesPerSecond = 87.307f }),
                    new Note(NotesNames.Fs2,
                    new Hz() { CyclesPerSecond = 92.499f }),
                    new Note(NotesNames.G2,
                    new Hz() { CyclesPerSecond = 97.999f }),
                    new Note(NotesNames.Ab2,
                    new Hz() { CyclesPerSecond = 103.826f }),
                    new Note(NotesNames.A2,
                    new Hz() { CyclesPerSecond = 110.000f }),                         //Open string number 5 - A2.
                    new Note(NotesNames.Bb2,
                    new Hz() { CyclesPerSecond = 116.541f }),
                    new Note(NotesNames.B2,
                    new Hz() { CyclesPerSecond = 123.471f }),
                    new Note(NotesNames.C3,
                    new Hz() { CyclesPerSecond = 130.813f }),
                    new Note(NotesNames.Cs3,
                    new Hz() { CyclesPerSecond = 138.591f }),
                    new Note(NotesNames.D3,
                    new Hz() { CyclesPerSecond = 146.832f }),                         //Open string number 4 - D3.
                    new Note(NotesNames.Eb3,
                    new Hz() { CyclesPerSecond = 155.563f }),
                    new Note(NotesNames.E3,
                    new Hz() { CyclesPerSecond = 164.814f }),
                    new Note(NotesNames.F3,
                    new Hz() { CyclesPerSecond = 174.614f }),
                    new Note(NotesNames.Fs3,
                    new Hz() { CyclesPerSecond = 184.997f }),
                    new Note(NotesNames.G3,
                    new Hz() { CyclesPerSecond = 184.997f }),                         //Open string number 3 - G3.
                    new Note(NotesNames.Ab3,
                    new Hz() { CyclesPerSecond = 207.652f }),
                    new Note(NotesNames.A3,
                    new Hz() { CyclesPerSecond = 220.000f }),
                    new Note(NotesNames.Bb3,
                    new Hz() { CyclesPerSecond = 233.082f }),
                    new Note(NotesNames.B3,
                    new Hz() { CyclesPerSecond = 246.942f }),                         //Open string number 2 - B3
                    new Note(NotesNames.C4,
                    new Hz() { CyclesPerSecond = 261.626f }),
                    new Note(NotesNames.Cs4,
                    new Hz() { CyclesPerSecond = 277.183f }),
                    new Note(NotesNames.D4,
                    new Hz() { CyclesPerSecond = 293.665f }),
                    new Note(NotesNames.Eb4,
                    new Hz() { CyclesPerSecond = 311.127f }),
                    new Note(NotesNames.E4,
                    new Hz() { CyclesPerSecond = 329.628f }),                         //Open string number 1 - E4.
                    new Note(NotesNames.F4,
                    new Hz() { CyclesPerSecond = 349.228f }),
                    new Note(NotesNames.Fs4,
                    new Hz() { CyclesPerSecond = 369.994f }),
                    new Note(NotesNames.G4,
                    new Hz() { CyclesPerSecond = 391.995f }),
                    new Note(NotesNames.Ab4,
                    new Hz() { CyclesPerSecond = 415.305f }),
                    new Note(NotesNames.A4,
                    new Hz() { CyclesPerSecond = 440.000f }),
                    new Note(NotesNames.Bb4,
                    new Hz() { CyclesPerSecond = 466.164f }),
                    new Note(NotesNames.B4,
                    new Hz() { CyclesPerSecond = 493.883f }),
                    new Note(NotesNames.C5,
                    new Hz() { CyclesPerSecond = 523.251f }),
                    new Note(NotesNames.Cs5,
                    new Hz() { CyclesPerSecond = 554.365f }),
                    new Note(NotesNames.D5,
                    new Hz() { CyclesPerSecond = 587.330f }),
                    new Note(NotesNames.Eb5,
                    new Hz() { CyclesPerSecond = 622.254f }),
                    new Note(NotesNames.E5,
                    new Hz() { CyclesPerSecond = 659.255f }),
                    new Note(NotesNames.F5,
                    new Hz() { CyclesPerSecond = 698.456f }),
                    new Note(NotesNames.Fs5,
                    new Hz() { CyclesPerSecond = 739.989f }),
                    new Note(NotesNames.G5,
                    new Hz() { CyclesPerSecond = 783.991f }),
                    new Note(NotesNames.Ab5,
                    new Hz() { CyclesPerSecond = 830.609f }),
                    new Note(NotesNames.A5,
                    new Hz() { CyclesPerSecond = 880.000f }),
                    new Note(NotesNames.Bb5,
                    new Hz() { CyclesPerSecond = 932.328f }),
                    new Note(NotesNames.B5,
                    new Hz() { CyclesPerSecond = 987.767f }),
                };
            }
        }

    /// <summary>
    /// Defines the six guitar open strings' frequencies.
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
}
