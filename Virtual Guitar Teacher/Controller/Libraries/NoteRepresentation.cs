using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Animation;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    /// <summary>
    /// Master class of Note.
    /// Represents a note visually by a colored ball and a number of string in its center.
    /// </summary>
    public class NoteRepresentation : IDisposable
    {
        private const int BUTTON_HEIGHT = 48, BUTTON_WIDTH = 48;

        private TextView _noteCircle;
        private Activity _currentActivity;
        private Note _note;

        //public TextView UndelyingViewObject{ get { return _noteCircle; } }
        public Note Note { get { return _note; } }

		//OnNoteArraival - once the note arrives to it's destination line.
		public event EventHandler<OnNoteArraivalArgs> OnNoteArraival;
		//OnNoteGone - once the note representation is no longer in view.
		public event EventHandler<EventArgs> OnNoteGone;

		// Special EventArgs class to hold info about...
		public class OnNoteArraivalArgs : EventArgs
		{
			private Hz _frequency;

			public OnNoteArraivalArgs(Hz frequency)
			{
				_frequency = frequency;
			}

			public Hz Frequency
			{
				get { return _frequency; }
			}
		}

        /// <summary>
        /// Constructs a new representation as a visual ball with a number of string in its center.
        /// </summary>
        /// <param name="currentActivity">The activity on which the ball should be drawn.</param>
        /// <param name="note">The note that should be played correspondingly when visually arriving to the guitar's neck.</param>
        public NoteRepresentation(Activity currentActivity, Note note)
        {
            //Basic values.
            _currentActivity = currentActivity;
            _note = note;

            //The color of the ball.
			BallColor ballColor = (BallColor)note.Position.String; //note.Position.Fret;
            //Which string the number on the ball should represent.
            //GuitarString stringNum = note.Position.String;

            _noteCircle = new TextView(_currentActivity);

            //Select the ball's color.
            switch (ballColor)
            {
                case BallColor.Red:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_red);
                    break;
                case BallColor.Purple:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_purple);
                    break;
                case BallColor.Blue:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_blue);
                    break;
                case BallColor.Green:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_green);
                    break;
                case BallColor.Orange:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_orange);
                    break;
                case BallColor.Aqua:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_aqua);
                    break;
                case BallColor.Yellow:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_yellow);
                    break;
            }

            //Set dimensions.
            _noteCircle.Layout(0, 0, BUTTON_WIDTH, BUTTON_HEIGHT);
            //Center text.
            _noteCircle.Gravity = GravityFlags.Center;
            //Set text.
            //_noteCircle.SetText(((int)stringNum).ToString(), TextView.BufferType.Normal);
            //Refresh view.
            _noteCircle.RequestLayout();
            
            //Add the newly created note circle to the current activity.
            _currentActivity.AddContentView(_noteCircle, new ViewGroup.LayoutParams(BUTTON_WIDTH, BUTTON_HEIGHT));
        }

        /// <summary>
        /// Animates the note from the bottom to the top, and positions it depending on its fret position.
        /// </summary>
        /// <returns>Returns the animation of this NoteRepresentation.</returns>
        public ObjectAnimator CreateNoteAnimation()
        {
            const float TRANSPARENT = 0;
            DisplayMetrics screenDimensions = Generic.GetScreenDimentions(_currentActivity);

            FretMetrics fretMetrics = GetFretMetrics(_note.Position.Fret);

            //TODO: Add string positions as well.

            //Set initial location.
			_noteCircle.SetX(fretMetrics.GetHorizontalCenter() - _noteCircle.Width / 2);
            _noteCircle.SetY(screenDimensions.HeightPixels);

			int Y_dest = fretMetrics.GetVerticalCenter() - _noteCircle.Height / 2;

            ObjectAnimator objAnim = ObjectAnimator.OfFloat(_noteCircle, "Y", Y_dest);
            objAnim.SetDuration((long)_note.Duration);
            objAnim.StartDelay = (long)_note.Delay;

            objAnim.AnimationEnd += (object sender, EventArgs e) => 
            {
                ObjectAnimator objAnimFadeOut = ObjectAnimator.OfFloat(_noteCircle, "Alpha", TRANSPARENT);
                objAnimFadeOut.SetDuration((long)_note.Duration);
                objAnimFadeOut.Start();

                //Start capturing of note's sound input.
				OnNoteArraival(this, new OnNoteArraivalArgs(_note.Hertz));

                objAnimFadeOut.AnimationEnd += (object sender2, EventArgs e2) =>
                {
                    //Note's sound input window closes.
					OnNoteGone(this, new EventArgs());
                };
            };

            return objAnim;

            /*PropertyViewAnimator pva = new PropertyViewAnimator(_noteCircle);
            //Start
            pva.Animate()
                .SetDuration((int)(_note.Duration))
                .SetStartDelay((int)(_note.Delay))
                .Y(Y_dest_start)
                .WithEndAction(new Runnable(() => {
                    //Start capturing of note's sound input.
                    //Raise StartNoteCapture event.
                    pva.Alpha(transparent).WithEndAction(new Runnable(() => {
                        //Note's sound input window closes.
                        //Raise EndNoteCapture event.
                    }));
                }));
            //Dilemma:
            //Middle and End.
            //But the animation does not happen here, it happens in AnimatorSet.

            return pva;*/
        }

        public ObjectAnimator CreateNoteAnimation_FadeOnly(Position position)
        {
            const float TRANSPARENT = 0;
            const long DEFAULT_DURATION = 1000;
            DisplayMetrics screenDimensions = Generic.GetScreenDimentions(_currentActivity);
            //TODO: Add string positions as well.

            FretMetrics fretMetrics = GetFretMetrics(position.Fret);
            int Y_dest = fretMetrics.GetVerticalCenter() - _noteCircle.Height / 2;
            int X_dest = fretMetrics.GetHorizontalCenter() - _noteCircle.Width / 2;
            //Set initial location.
            _noteCircle.SetX(X_dest);
            _noteCircle.SetY(Y_dest);

            ObjectAnimator objAnimFadeOut = ObjectAnimator.OfFloat(_noteCircle, "Alpha", TRANSPARENT);
            objAnimFadeOut.SetDuration(DEFAULT_DURATION);

            objAnimFadeOut.AnimationEnd += (object sender, EventArgs e) =>
            {
                OnNoteGone(this, new EventArgs());
            };

            return objAnimFadeOut;
        }

        public void SetText(string text)
        {
            _noteCircle.Text = text;
        }

        /// <summary>
        /// Gets the X, Y, Width, and Height of the fret which is represented on the screen by a view.
        /// </summary>
        /// <param name="fretNum">The number of the requested fret.</param>
        /// <returns>Returns the metrics of the requested fret.</returns>
        private FretMetrics GetFretMetrics(GuitarFret fretNum)
        {
            //Initialize a dummy fret.
            View fret = new View(_currentActivity);

            //Get the correct fret number.
            /*if (_currentActivity.Title == "TutorActivity")

            else if (_currentActivity.Title == "PlayerActivity")

            else if (_currentActivity.Title == "RecorderActivity")*/

            switch (fretNum)
            {
                case GuitarFret.OpenString:
                        fret = _currentActivity.FindViewById(Resource.Id.openString);
                    break;
                case GuitarFret.Fret1:
                        fret = _currentActivity.FindViewById(Resource.Id.fret1);
                    break;
                case GuitarFret.Fret2:
                        fret = _currentActivity.FindViewById(Resource.Id.fret2);
                    break;
                case GuitarFret.Fret3:
                        fret = _currentActivity.FindViewById(Resource.Id.fret3);
                    break;
                case GuitarFret.Fret4:
                        fret = _currentActivity.FindViewById(Resource.Id.fret4);
                    break;
                case GuitarFret.Fret5:
                        fret = _currentActivity.FindViewById(Resource.Id.fret5);
                    break;
                case GuitarFret.Fret6:
                        fret = _currentActivity.FindViewById(Resource.Id.fret6);
                    break;
            }

            //Get the X & Y coordinates of the fret on the screen.
            int[] coordinates = new int[2];
            fret.GetLocationInWindow(coordinates);
            
            //Summerize X, Y, Width, and Height.
            FretMetrics metrics = new FretMetrics(
                (int)fret.GetX(), //coordinates[0],
				(int)((TableRow)fret.Parent).GetY(), //coordinates[1],
                fret.MeasuredWidth,
                fret.MeasuredHeight);

            Log.Info("", fret.Tag.ToString());
            Log.Info(fret.GetX().ToString(), fret.GetY().ToString());
            Log.Info(coordinates[0].ToString(), coordinates[1].ToString());
            Log.Info(fret.Width.ToString(), fret.Height.ToString());
            Log.Info("Left", fret.Left.ToString());
            Log.Info("Right", fret.Right.ToString());
            Log.Info("Top", fret.Top.ToString());
            Log.Info("Bottom", fret.Bottom.ToString());

            return metrics;
        }

		public void ChangeToSmileyFace(bool isHappy)
		{
			if (isHappy)
				_noteCircle.SetBackgroundResource(Resource.Drawable.smiley_happy);
			else
				_noteCircle.SetBackgroundResource(Resource.Drawable.smiley_sad);
		}

        public void Dispose()
        {
            _noteCircle.Dispose();
        }
    }
}