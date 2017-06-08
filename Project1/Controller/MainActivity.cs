using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using DisertationProject.Model;
using System.Collections.Generic;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = Globals.ProjectLabel, MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class MainActivity : Activity
    {
        /// <summary>
        /// The instance of the main activity
        /// </summary>
        private static MainActivity instance { get; set; }

        /// <summary>
        /// Get instance of main activity
        /// </summary>
        /// <returns>Instance of main activity</returns>
        public static MainActivity getInstace()
        {
            return instance;
        }

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
        private IDictionary<int, Button> buttons;

        /// <summary>
        /// Broadcast receiver fro the music service
        /// </summary>
        private MusicServiceListener receiver;

        /// <summary>
        /// Text controller used to controller text in text containers
        /// </summary>
        private TextController textCotroller;

        /// <summary>
        /// Initialization method
        /// </summary>
        private void Initialize()
        {
            Context = ApplicationContext;
            Connectivity = ConnectivityService;
            Wifi = WifiService;
            Audio = AudioService;

            buttons = new Dictionary<int, Button>();
            textCotroller = new TextController(instance);
            receiver = new MusicServiceListener();

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
            //Set instance
            instance = this;

            Initialize();
            SetupButtons();

            var intentFilter = new IntentFilter();
            //intentFilter.AddAction(Globals.TestAction);
            intentFilter.AddAction(Globals.NetworkOffline);
            //intentFilter.AddAction(Globals.TheAction);
            RegisterReceiver(receiver, intentFilter);
            textCotroller.Initialize();
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
            Helper.addItemToDictionary<int, Button>(buttons, buttonList, FindViewById<Button>);
            Helper.addItemToDictionary<int, Button>(buttons, Globals.RepeatButtonId, FindViewById<ToggleButton>);

            //Set click action
            buttons[Globals.PlayButtonId].Click += (sender, args) => SendCommand(Globals.ActionPlay);
            buttons[Globals.PauseButtonId].Click += (sender, args) => SendCommand(Globals.ActionPause);
            buttons[Globals.StopButtonId].Click += (sender, args) => SendCommand(Globals.ActionStop);
            buttons[Globals.PreviousButtonId].Click += (sender, args) => SendCommand(Globals.ActionPrevious);
            buttons[Globals.NextButtonId].Click += (sender, args) => SendCommand(Globals.ActionNext);
            buttons[Globals.RepeatButtonId].Click += (sender, args) =>
            {
                if (((ToggleButton)buttons[Globals.RepeatButtonId]).Checked) SendCommand(Globals.ActionRepeatOn);
                else SendCommand(Globals.ActionRepeatOff);
            };
        }

        /// <summary>
        /// Set _command method
        /// </summary>
        /// <_parameter name="action">The action that is intended</_parameter>
        private void SendCommand(string action)
        {
            var intent = new Intent(action);
            textCotroller.RefreshList();
            Context.StartService(intent);
        }

        /// <summary>
        /// Override OnDestroy method
        /// </summary>
        protected override void OnDestroy()
        {
            UnregisterReceiver(receiver);
            base.OnDestroy();
        }

        /// <summary>
        /// Override OnPause method
        /// </summary>
        protected override void OnPause()
        {
            UnregisterReceiver(receiver);
            base.OnPause();
        }

        public void SetButtonText(string text)
        {
            //_errorTextBox.Text = text;
            //textCotroller.TextContainer.SetText("It's ok");
           
        }
    }

    /// <summary>
    /// Broadcast receiver class which is used to listen to messages from the music service
    /// </summary>
    [BroadcastReceiver]
    [IntentFilter(new[] { Globals.TestAction, Globals.NetworkOffline, Globals.IllegalStateException })]
    public class MusicServiceListener : BroadcastReceiver
    {
        /// <summary>
        /// Method which handles the received intent
        /// </summary>
        /// <param name="context">The context in which it is used</param>
        /// <param name="intent">The intent</param>
        public override void OnReceive(Context context, Intent intent)
        {
            string text;
            switch (intent.Action)
            {
                case Globals.TestAction:
                    text = "It's ok";
                    break;
                case Globals.NetworkOffline:
                    text = "Network is Offline";
                    break;
                default:
                    text = "don't know ?";
                    break;
            }
            //MainActivity.getInstace().SetButtonText(text);
            InvokeAbortBroadcast();
        }
    }
}