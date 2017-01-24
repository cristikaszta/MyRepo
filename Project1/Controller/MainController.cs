using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Widget;
using DisertationProject.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Android.Runtime;
using System.Threading.Tasks;
using System.Text;


namespace DisertationProject.Controller
{
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = Globals.ProjectLabel, MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class MainController : Activity
    {

        //SqlConnection sqlconn;
        //string connsqlstring = string.Format("Server=tcp:ourserver.database.windows.net,1433;Data Source=ourserver.database.windows.net;Initial Catalog=ourdatabase;Persist Security Info=False;User ID=lanister;Password=tyrion0!;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;");

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

        public SomeBroadcastReceiver _receiver;

        private TextView _errorTextBox;

        /// <summary>
        /// Object initialization method
        /// </summary>
        private void Initialize()
        {

            Context = ApplicationContext;
            Connectivity = ConnectivityService;
            Wifi = WifiService;
            Audio = AudioService;

            _buttons = new Dictionary<int, Button>();
            _errorTextBox = (TextView)FindViewById(Globals.ErrorTextBox);    
            SetupButtons();

        }
        

        /// <summary>
        /// On create method
        /// </summary>
        /// <_parameter name="bundle">The bundle</_parameter>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Globals.MainLayoutId);
       
            Initialize();

            _receiver = new SomeBroadcastReceiver();
            IntentFilter _intentFilter = new IntentFilter();
            _intentFilter.AddAction(Globals.TheAction);
            Context.RegisterReceiver(_receiver, _intentFilter);
        }

        /// <summary>
        /// Setup buttons method
        /// </summary>
        private void SetupButtons()
        {
            var buttonList = new List<int>
            {
               Globals.PlayButtonId,
               Globals.PauseButtonId,
               Globals.StopButtonId,
               Globals.PreviousButtonId,
               Globals.NextButtonId,
            };
            //Create the dictionaries
            Helper.addItemToDictionary<int, Button>(_buttons, buttonList, FindViewById<Button>);
            Helper.addItemToDictionary<int, Button>(_buttons, Globals.RepeatButtonId, FindViewById<ToggleButton>);

            //Set click action
            _buttons[Globals.PlayButtonId].Click += (sender, args) => SendCommand(Globals.ActionPlay);
            _buttons[Globals.PauseButtonId].Click += (sender, args) => SendCommand(Globals.ActionPause);
            _buttons[Globals.StopButtonId].Click += (sender, args) => SendCommand(Globals.ActionStop);
            _buttons[Globals.PreviousButtonId].Click += (sender, args) => SendCommand(Globals.ActionPrevious);
            _buttons[Globals.NextButtonId].Click += (sender, args) => SendCommand(Globals.ActionNext);
            _buttons[Globals.RepeatButtonId].Click += (sender, args) =>
            {
                if (((ToggleButton)_buttons[Globals.RepeatButtonId]).Checked) SendCommand(Globals.ActionRepeatOn);
                else SendCommand(Globals.ActionRepeatOff);
            };
        }

        /// <summary>
        /// Set _command method
        /// </summary>
        /// <_parameter name="action">The action</_parameter>
        private void SendCommand(string action)
        {
            _errorTextBox.Text = "Playing";
            var _intent = new Intent(action);
            StartService(_intent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _receiver.UnregisterFromRuntime();
        }
    }
}