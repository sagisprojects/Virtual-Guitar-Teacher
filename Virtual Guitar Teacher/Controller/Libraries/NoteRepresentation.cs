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

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    /// <summary>
    /// Represents a note visually by a colored ball and a number of string in its center.
    /// </summary>
    public class NoteRepresentation
    {
        private const int BUTTON_HEIGHT = 48, BUTTON_WIDTH = 48;

        private TextView _noteCircle;
        private Activity _currentActivity;
        private Note _note;

        /// <summary>
        /// Constructs a new representation as a visual ball with a number of string in its center.
        /// </summary>
        /// <param name="currentActivity">The activity on which the ball should be drawn.</param>
        /// <param name="ballColor">The color of the ball.</param>
        /// <param name="stringNum">Which string the number on the ball should represent.</param>
        /// <param name="note">The note that should be played correspondingly when visually arriving to the guitar's neck.</param>
        public NoteRepresentation(Activity currentActivity, BallColor ballColor, GuitarString stringNum, Note note)
        {
            //Basic values.
            _currentActivity = currentActivity;
            _note = note;

            _noteCircle = new TextView(_currentActivity);

            //Select the ball's color.
            switch (ballColor)
            {
                case BallColor.Blue:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_blue);
                    break;
                case BallColor.Green:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_green);
                    break;
                case BallColor.Orange:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_orange);
                    break;
                case BallColor.Purple:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_purple);
                    break;
                case BallColor.Red:
                    _noteCircle.SetBackgroundResource(Resource.Drawable.ball_red);
                    break;
            }

            //Set dimensions.
            _noteCircle.Layout(0, 0, BUTTON_WIDTH, BUTTON_HEIGHT);
            //Center text.
            _noteCircle.Gravity = GravityFlags.Center;
            //Set text.
            _noteCircle.SetText(stringNum.ToString(), TextView.BufferType.Normal);
            //Refresh view.
            _noteCircle.RequestLayout();
            
            //Add the newly created note circle to the current activity.
            _currentActivity.AddContentView(_noteCircle, new ViewGroup.LayoutParams(BUTTON_WIDTH, BUTTON_HEIGHT));
        }

        /// <summary>
        /// Animates the note from the bottom to the top, and positions it depending on its fret position.
        /// </summary>
        public void AnimateNote(long duration, long delay)
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
        }

    }
}