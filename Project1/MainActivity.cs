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
    [Activity(Label = "DisertationProject", MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class MainActivity : Activity
    {
        /// <summary>
        /// Dictionary of the buttons;
        /// Each item contains a key and a value
        /// </summary>
        private IDictionary<int, Button> Buttons;

        /// <summary>
        /// Initialize a dictionary
        /// </summary>
        /// <typeparam name="T1">Parameter type 1</typeparam>
        /// <typeparam name="T2">Parameter type 2</typeparam>
        /// <param name="idList">List of dictionary ids</param>
        /// <param name="callMethod">The method to be called to initialize the values for each item in the dictionary</param>
        /// <returns>Returns a dictionary object with T1 type key and T2 type value</returns>
        public IDictionary<T1, Button> InitializeDictionary<T1, T2>(List<T1> idList, Func<T1, Button> callMethod) where T2 : Dictionary<T1, Button>, new()
        {
            var dictionary = new Dictionary<T1, Button>();

            foreach (var id in idList)
            {
                var t = callMethod(id);
                Button y = FindViewById<Button>(Resource.Id.pauseButton);
                dictionary.Add(id, t);
            }
            return dictionary;
        }

        /// <summary>
        /// Object initialization method
        /// </summary>
        private void Initialize()
        {

            // Set our view from the "main" layout resource
            SetContentView(Globals.MainLayoutId);

            //Create the buttons id list
            var buttonsIds = new List<int>();
            buttonsIds.Add(Globals.PlayButtonId);
            buttonsIds.Add(Globals.PauseButtonId);
            buttonsIds.Add(Globals.StopButtonId);

            // Attribute assignation
            Buttons = InitializeDictionary<int, Dictionary<int, Button>>(buttonsIds, FindViewById<Button>);
        }

        /// <summary>
        /// On create method
        /// </summary>
        /// <param name="bundle">The bundle</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Initialize();

            //Set click action
            Buttons[Globals.PlayButtonId].Click += (sender, args) => SendAudioCommand(Globals.ActionPlay);
            Buttons[Globals.PauseButtonId].Click += (sender, args) => SendAudioCommand(Globals.ActionPause);
            Buttons[Globals.StopButtonId].Click += (sender, args) => SendAudioCommand(Globals.ActionStop);
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


