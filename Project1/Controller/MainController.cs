using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using DisertationProject.Data;
using System.Collections.Generic;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = Globals.ProjectLabel, MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class MainController : Activity
    {
        /// <summary>
        /// Main activity context
        /// </summary>
        public static Context Context;

        /// <summary>
        /// Main activity context Audio service
        /// </summary>
        public static string Audio;

        /// <summary>
        /// Main activity context Wifi service
        /// </summary>
        public static string Wifi;

        /// <summary>
        /// The conectivity context
        /// </summary>
        public static string Connectivity;

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
        /// Object initialization method
        /// </summary>
        private void Initialize()
        {
            //The context of the single, global Application object of the current process.
            Context = ApplicationContext;
            //The android services
            Connectivity = ConnectivityService;
            Wifi = WifiService;
            Audio = AudioService;

            _common = new CommonController();
            _buttons = new Dictionary<int, Button>();
            SetupButtons();
        }

        /// <summary>
        /// Setup buttons method
        /// </summary>
        private void SetupButtons()
        {
            //Create the dictionaries
            _common.addItemToDictionary<int, Button>(_buttons, Globals.PlayButtonId, FindViewById<Button>);
            _common.addItemToDictionary<int, Button>(_buttons, Globals.PauseButtonId, FindViewById<Button>);
            _common.addItemToDictionary<int, Button>(_buttons, Globals.StopButtonId, FindViewById<Button>);
            _common.addItemToDictionary<int, Button>(_buttons, Globals.PreviousButtonId, FindViewById<Button>);
            _common.addItemToDictionary<int, Button>(_buttons, Globals.NextButtonId, FindViewById<Button>);
            _common.addItemToDictionary<int, Button>(_buttons, Globals.RepeatButtonId, FindViewById<ToggleButton>);

            //Set click action
            _buttons[Globals.PlayButtonId].Click += (sender, args) => SendCommand(Globals.ActionPlay);
            _buttons[Globals.PauseButtonId].Click += (sender, args) => SendCommand(Globals.ActionPause);
            _buttons[Globals.StopButtonId].Click += (sender, args) => SendCommand(Globals.ActionStop);
            _buttons[Globals.PreviousButtonId].Click += (sender, args) => SendCommand(Globals.ActionPrevious);
            _buttons[Globals.NextButtonId].Click += (sender, args) => SendCommand(Globals.ActionNext);
            _buttons[Globals.RepeatButtonId].Click += (sender, args) =>
            {
                if (((ToggleButton)_buttons[Globals.RepeatButtonId]).Checked)
                    SendCommand(Globals.ActionRepeatOn);
                else
                    SendCommand(Globals.ActionRepeatOff);
            };
        }

        /// <summary>
        /// Set command method
        /// </summary>
        /// <param name="action">The action</param>
        private void SendCommand(string action)
        {
            var _intent = new Intent(action);
            StartService(_intent);
        }
    }
}