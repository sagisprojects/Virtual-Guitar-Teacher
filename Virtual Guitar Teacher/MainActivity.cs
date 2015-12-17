using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Virtual_Guitar_Teacher
{
    [Activity(Label = "Virtual Guitar Teacher", Icon = "@drawable/icon", 
        MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash")]
	/* INFO:
		MainLauncher 			- This is the activity that should be launched when the user clicks our icon.
		MainLauncher = true 	- This controls if this activity has an icon in the application launcher.
		Theme 					- Tell android to use our theme we made for this activity.
		NoHistory 				- Tell android not to put the activity in the 'back stack', 
									that is, when the user hits the back button from the real application, 
									don't show this activity again.
	*/
    public class MainActivity : Activity
    {
        //int count = 1;
        public static Context appContext;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            /*ActionBar actionBar = getActionBar();
            actionBar.setSubtitle("mytest");
            actionBar.setTitle("vogella.com");*/


            /*ActionBar.Tab tab1 = new ActionBar.Tab();
            this.ActionBar.AddTab();*/


            Window.AddFlags(WindowManagerFlags.ShowWhenLocked); //If device is locked, and soft lock is on, show this window.
            //Window.AddFlags(WindowManagerFlags.DismissKeyguard);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);   //Keep on shining you crazy diamond.
            Window.AddFlags(WindowManagerFlags.Fullscreen);     //Hide all screen decorations (such as the statusbar).
            //Window.AddFlags(WindowManagerFlags.LayoutInScreen); //Hide status bar (clock, antenna reception, battery level).

            RequestedOrientation = Android.Content.PM.ScreenOrientation.SensorLandscape;
            RequestWindowFeature(WindowFeatures.NoTitle);


            // Activate the action bar and display it in navigation mode.
            //RequestWindowFeature(WindowFeatures.ActionBar);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            /*Toolbar myToolbar = (Toolbar)findViewById(R.id.my_toolbar);
            setSupportActionBar(myToolbar);*/

            // Get our button from the layout resource,
            // and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.MyButton);

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

            // Initialize the action bar
            //InitializeActionBar();
        }

        /*private void InitializeActionBar()
        {
            // Set the action bar to Tab mode
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            // Create a new tab (CreateTabSpec on the TabControl)
            var homeTab = ActionBar.NewTab();

            homeTab.SetTabListener(
                new TabListener<CrumbsOverviewFragment>());

            homeTab.SetText("Crumbs");

            // Add the new tab to the action bar
            ActionBar.AddTab(homeTab);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            /*MenuInflater menuInflater = getMenuInflater();
            menuInflater.inflate(R.menu.actionmenu, menu);*/
            /*
            MenuInflater.Inflate(Resource.Menu.ActionItems, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //TODO: Handle the selection event here.
            return false;
        }*/
    }

    /* OBSOLETE
    /// <summary>
    /// Listener that handles the selection of a tab in the user interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TabListener<T> : Java.Lang.Object, ActionBar.ITabListener
        where T : Fragment, new()
    {
        private T _fragment;

        /// <summary>
        /// initializes a new instance of the tab listener
        /// </summary>
        public TabListener()
        {
            _fragment = new T();
        }

        /// <summary>
        /// Initializes a new instance of the tab listener
        /// </summary>
        /// <param name="fragment"></param>
        protected TabListener(T fragment)
        {
            _fragment = fragment;
        }

        /// <summary>
        /// Handles the reselection of the tab
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="ft"></param>
        public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction ft)
        {

        }

        /// <summary>
        /// Adds the fragment when the tab was selected
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="ft"></param>
        public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction ft)
        {//Resource.Id.MainFragmentContainer
            ft.Add(Resource.Id.menu_guitar, _fragment, typeof(T).FullName);
        }

        /// <summary>
        /// Removes the fragment when the tab was deselected
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="ft"></param>
        public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction ft)
        {
            ft.Remove(_fragment);
        }
    }*/
}

