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
using Virtual_Guitar_Teacher.Controller;

namespace Virtual_Guitar_Teacher.Activities
{
    [Activity(Label = "PlayerActivity")]
    public class PlayerActivity : BasicActivityInitialization
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set appropriate layout.
            SetContentView(Resource.Layout.Player);

            Player play = new Player(this);
        }
    }
}