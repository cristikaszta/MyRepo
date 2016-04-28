using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using DisertationProject.Data;
using System.Collections;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = Globals.ProjectLabel, MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class MainActivity : Activity
    {
        /// <summary>
        /// Dictionary of the buttons;
        /// Each item contains a key and a value
        /// </summary>
        private IDictionary<int, Button> _buttons;

        /// <summary>
        /// Common controller object
        /// </summary>
        private CommonController _common;

        /// <summary>
        /// Object initialization method
        /// </summary>
        private void Initialize()
        {
            _common = new CommonController();
            _buttons = new Dictionary<int, Button>();

            //Create the dictionaries
            _common.addItemToDictionary<int, Button>(_buttons, Globals.PlayButtonId, FindViewById<Button>);
            _common.addItemToDictionary<int, Button>(_buttons, Globals.PauseButtonId, FindViewById<Button>);
            _common.addItemToDictionary<int, Button>(_buttons, Globals.StopButtonId, FindViewById<Button>);

            //Set click action
            _buttons[Globals.PlayButtonId].Click += (sender, args) => SendAudioCommand(Globals.ActionPlay);
            _buttons[Globals.PauseButtonId].Click += (sender, args) => SendAudioCommand(Globals.ActionPause);
            _buttons[Globals.StopButtonId].Click += (sender, args) => SendAudioCommand(Globals.ActionStop);
        }

       

        /// <summary>
        /// On create method
        /// </summary>
        /// <param name="bundle">The bundle</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Globals.MainLayoutId);

            //Initialize the attributes
            Initialize();

        }


        /// <summary>
        /// Set audio command method
        /// </summary>
        /// <param name="action">The action</param>
        private void SendAudioCommand(string action)
        {
            var intent = new Intent(action);
            StartService(intent);
        }
    }
}


