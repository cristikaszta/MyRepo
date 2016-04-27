using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using DisertationProject.Services.StreamingService;
using System.Collections.Generic;
using DisertationProject.Data;
using System.Collections;

namespace DisertationProject
{
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = Globals.ProjectLabel, MainLauncher = true, Icon = Globals.ProjectIcon)]
    public class MainActivity : Activity
    {
        /// <summary>
        /// Dictionary of the buttons;
        /// Each item contains a key and a value
        /// </summary>
        private IDictionary<int, Button> Buttons;

        /// <summary>
        /// Object initialization method
        /// </summary>
        private void Initialize()
        {

            //Create the dictionaries
            addItemToDictionary<int, Button>(Buttons, Globals.PlayButtonId, FindViewById<Button>);
            addItemToDictionary<int, Button>(Buttons, Globals.PauseButtonId, FindViewById<Button>);
            addItemToDictionary<int, Button>(Buttons, Globals.StopButtonId, FindViewById<Button>);

            //Set click action
            Buttons[Globals.PlayButtonId].Click += (sender, args) => SendAudioCommand(Globals.ActionPlay);
            Buttons[Globals.PauseButtonId].Click += (sender, args) => SendAudioCommand(Globals.ActionPause);
            Buttons[Globals.StopButtonId].Click += (sender, args) => SendAudioCommand(Globals.ActionStop);
        }

        /// <summary>
        /// Method used to add items to dictionaries
        /// </summary>
        /// <typeparam name="T1">Dictionary key type parameter</typeparam>
        /// <typeparam name="T2">Dictionary value type parameter</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public void addItemToDictionary<T1, T2>(IDictionary<T1, T2> dictionary, T1 key, Func<T1, T2> value)
        {
            dictionary.Add(key, (T2)value(key));
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


