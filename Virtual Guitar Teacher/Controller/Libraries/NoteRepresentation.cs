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
using Android.Util;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views.Animations;
using Android.Animation;
using Java.Lang;

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
            BallColor ballColor = (BallColor)note.Position.Fret;
            //Which string the number on the ball should represent.
            GuitarString stringNum = note.Position.String;

            _noteCircle = new TextView(_currentActivity);

            //Select the ball's color.
            switch (ballColor)
            {
                case BallColor.Red:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_red);
                    break;
                case BallColor.Blue:
                case BallColor.Blue2:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_blue);
                    break;
                case BallColor.Green:
                case BallColor.Green2:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_green);
                    break;
                case BallColor.Orange:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_orange);
                    break;
                case BallColor.Purple:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_purple);
                    break;
            }

            //Set dimensions.
            _noteCircle.Layout(0, 0, BUTTON_WIDTH, BUTTON_HEIGHT);
            //Center text.
            _noteCircle.Gravity = GravityFlags.Center;
            //Set text.
            _noteCircle.SetText(((int)stringNum).ToString(), TextView.BufferType.Normal);
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
            _noteCircle.SetX(fretMetrics.GetHorizontalCenter());
            _noteCircle.SetY(screenDimensions.HeightPixels);

            int Y_dest = fretMetrics.GetVerticalCenter();

            ObjectAnimator objAnim = ObjectAnimator.OfFloat(_noteCircle, "Y", Y_dest);
            objAnim.SetDuration((long)_note.Duration);
            objAnim.StartDelay = (long)_note.Delay;

            objAnim.AnimationEnd += (object sender, EventArgs e) => 
            {
                ObjectAnimator objAnimFadeOut = ObjectAnimator.OfFloat(_noteCircle, "Alpha", TRANSPARENT);
                objAnimFadeOut.SetDuration((long)_note.Duration);
                objAnimFadeOut.Start();

                //Start capturing of note's sound input.

                objAnimFadeOut.AnimationEnd += (object sender2, EventArgs e2) =>
                {
                    //Note's sound input window closes.

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

        /// <summary>
        /// Gets the X, Y, Width, and Height of the fret which is represented on the screen by a view.
        /// </summary>
        /// <param name="fretNum">The number of the requested fret.</param>
        /// <returns>Returns the metrics of the requested fret.</returns>
        FretMetrics GetFretMetrics(GuitarFret fretNum)
        {
            //Initialize a dummy fret.
            View fret = new View(_currentActivity);

            //Get the correct fret number.
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
                (int)fret.GetY(), //coordinates[1],
                fret.MeasuredWidth,
                fret.MeasuredHeight);

            Log.Info("", fret.Tag.ToString());
            Log.Info(fret.GetX().ToString(), fret.GetY().ToString());
            Log.Info(fret.MeasuredWidth.ToString(), fret.MeasuredHeight.ToString());
            Log.Info(fret.Width.ToString(), fret.Height.ToString());
            Log.Info("Left", fret.Left.ToString());
            Log.Info("Right", fret.Right.ToString());
            Log.Info("Top", fret.Top.ToString());
            Log.Info("Bottom", fret.Bottom.ToString());

            return metrics;
        }

        public void Dispose()
        {
            _noteCircle.Dispose();
        }

        //Not in use.
        private void AnimateNote(long duration, long delay)
        {
            DisplayMetrics dimensions = Generic.GetScreenDimentions(_currentActivity);

            //Set initial location.
            _noteCircle.SetX(dimensions.WidthPixels - _noteCircle.Width / 2 - 150);
            _noteCircle.SetY(dimensions.HeightPixels);           

            int Y_dest = dimensions.HeightPixels / 2;
            //_note.Position.Fret
            _noteCircle.Animate()
                .SetDuration(duration)
                .SetStartDelay(delay)
                .Y(Y_dest)
                .Start();



            //AnimationSet animationSet = new AnimationSet(true);

            /*ObjectAnimator noteAnim = ObjectAnimator.OfInt(_noteCircle, "Y", Y_dest);
            //animationSet.AddAnimation(noteAnim);
            List<ObjectAnimator> animations = new List<ObjectAnimator>();
            animations.Add(noteAnim);
            animatorSet.PlaySequentially((IList<Animator>)animations);*/

        }


        //Not in use.
        private class NoteAnimation : Animation
        {
            /*Duration
            FillAfter
                Initialize
            IsInitialized
            AnimationEnd
            AnimationRepeat
            AnimationStart

            Cancel
            Reset

            StartNow
            ApplyTransformation*/

            private ViewPropertyAnimator _viewPropertyAnimator;

            public NoteAnimation(ViewPropertyAnimator vpa)
            {
                _viewPropertyAnimator = vpa;
            }

            public override void StartNow()
            {
                base.StartNow();
                _viewPropertyAnimator.Start();
            }
        }
    }
}