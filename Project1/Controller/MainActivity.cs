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
    public class MainActivity : Activity
    {

        private static MainActivity _instance { get; set; }
        public static MainActivity getInstace()
        {
            return _instance;
        }

        #region Location

        //LOCATION
        //public async void OnLocationChanged(Location location)
        //{
        //    _currentLocation = location;
        //    if (_currentLocation == null)
        //    {
        //        _locationText.Text = "Unable to determine your location. Try again in a short while.";
        //    }
        //    else
        //    {
        //        _locationText.Text = string.Format("{0:f6},{1:f6}", _currentLocation.Latitude, _currentLocation.Longitude);
        //        Address address = await ReverseGeocodeCurrentLocation();
        //        DisplayAddress(address);

        //    }
        //}

        //public void OnProviderDisabled(string provider) { }

        //public void OnProviderEnabled(string provider) { }

        //public void OnStatusChanged(string provider, Availability status, Bundle extras)
        //{
        //    Log.Debug(TAG, "{0}, {1}", provider, status);
        //}

        //static readonly string TAG = "X:" + typeof(Activity).Name;
        //TextView _addressText;
        //Location _currentLocation;
        //LocationManager _locationManager;

        //string _locationProvider;
        //TextView _locationText;

        //SqlConnection sqlconn;
        //string connsqlstring = string.Format("Server=tcp:ourserver.database.windows.net,1433;Data Source=ourserver.database.windows.net;Initial Catalog=ourdatabase;Persist Security Info=False;User ID=lanister;Password=tyrion0!;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;");

        #endregion

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

        /// <summary>
        /// Broadcast receiver fro the music service
        /// </summary>
        private MusicServiceListener _receiver;

        private VisualFeedbackController _visualFeedbackController;

        //private TextView _errorTextBox;

        /// <summary>
        /// Object initialization method
        /// </summary>
        private void Initialize()
        {
            //Connect to DB
            //try
            //{
            //    sqlconn = new SqlConnection(connsqlstring);

            //    sqlconn.Open();
            //}
            //catch (Exception e)
            //{

            //}

            Context = ApplicationContext;
            Connectivity = ConnectivityService;
            Wifi = WifiService;
            Audio = AudioService;

            _buttons = new Dictionary<int, Button>();
            //_errorTextBox = (TextView)FindViewById(Globals.ErrorTextBox);
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
            //Set instance
            _instance = this;
            //LOCATION
            //_addressText = FindViewById<TextView>(Resource.Id.address_text);
            //_locationText = FindViewById<TextView>(Resource.Id.location_text);
            //FindViewById<TextView>(Resource.Id.get_address_button).Click += AddressButton_OnClick;

            //InitializeLocationManager();

            Initialize();
            _visualFeedbackController = new VisualFeedbackController(_instance);
            _receiver = new MusicServiceListener();
            var _intentFilter = new IntentFilter(Globals.TheAction);
            //_intentFilter.AddAction(Globals.TheAction);
            RegisterReceiver(_receiver, _intentFilter);

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

            //Long click to test the get song by id
            //_buttons[Globals.PlayButtonId].LongClick += (sender, args) => MainActivity_LongClick(16);
        }

        /// <summary>
        /// Set _command method
        /// </summary>
        /// <_parameter name="action">The action</_parameter>
        private void SendCommand(string action)
        {
           // _errorTextBox.Text = "Playing";
            var _intent = new Intent(action);
            StartService(_intent);
        }

        #region Location

        //LOCATION
        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        //    Log.Debug(TAG, "Listening for location updates using " + _locationProvider + ".");
        //}

        //protected override void OnPause()
        //{
        //    base.OnPause();
        //    _locationManager.RemoveUpdates(this);
        //    Log.Debug(TAG, "No longer listening for location updates.");
        //}

        //void InitializeLocationManager()
        //{
        //    _locationManager = (LocationManager)GetSystemService(LocationService);
        //    Criteria criteriaForLocationService = new Criteria
        //    {
        //        Accuracy = Accuracy.Fine
        //    };
        //    IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

        //    if (acceptableLocationProviders.Any())
        //    {
        //        _locationProvider = acceptableLocationProviders.First();
        //    }
        //    else
        //    {
        //        _locationProvider = string.Empty;
        //    }
        //    Log.Debug(TAG, "Using " + _locationProvider + ".");
        //}



        //LOCATION
        //async void AddressButton_OnClick(object sender, EventArgs eventArgs)
        //{
        //    if (_currentLocation == null)
        //    {
        //        _addressText.Text = "Can't determine the current address. Try again in a few minutes.";
        //        return;
        //    }

        //    Address address = await ReverseGeocodeCurrentLocation();
        //    DisplayAddress(address);
        //}

        //async Task<Address> ReverseGeocodeCurrentLocation()
        //{
        //    Geocoder geocoder = new Geocoder(this);
        //    IList<Address> addressList =
        //        await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

        //    Address address = addressList.FirstOrDefault();
        //    return address;
        //}

        //void DisplayAddress(Address address)
        //{
        //    if (address != null)
        //    {
        //        StringBuilder deviceAddress = new StringBuilder();
        //        for (int i = 0; i < address.MaxAddressLineIndex; i++)
        //        {
        //            deviceAddress.AppendLine(address.GetAddressLine(i));
        //        }
        //        // Remove the last comma from the end of the address.
        //        _addressText.Text = deviceAddress.ToString();
        //    }
        //    else
        //    {
        //        _addressText.Text = "Unable to determine the address. Try again in a few minutes.";
        //    }
        //}

        //private void MainActivity_LongClick(int playListId)
        //{
        //    GetSong(playListId);
        //}


        // _connection.Close();


        //WORK IN PROGRESS TO PLAY VIDEO FROM YOUTUBE LINK (HARCODED FOR NOW)
        //I THINK IT DOES NOT WORK IN MY EMULATOR , PLEASE TRY DIRECTLY IN THE PHONE

        //Obtain a reference to the VideoView in code.
        // var videoView = FindViewById<VideoView>(Resource.Id.YoutubeVideoView);
        // Create an Android.Net.Uri for the video.
        //var uri = Android.Net.Uri.Parse("rtsp://r2---sn-a5m7zu76.c.youtube.com/Ck0LENy73wIaRAnTmlo5oUgpQhMYESARFEgGUg5yZWNvbW1lbmRhdGlvbnIhAWL2kyn64K6aQtkZVJdTxRoO88HsQjpE1a8d1GxQnGDmDA==/0/0/0/video.3gp");
        // Pass this Uri to the VideoView.
        //videoView.SetVideoURI(uri);
        // Start the VideoView.
        //videoView.RequestFocus();
        //videoView.Start();

        //public void OnLocationChanged(Location location)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OnProviderDisabled(string provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OnProviderEnabled(string provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        protected override void OnDestroy()
        {
            UnregisterReceiver(_receiver);
            base.OnDestroy();
        }

        protected override void OnPause()
        {
            UnregisterReceiver(_receiver);
            base.OnPause();
        }

        public void SetButtonText(string text)
        {
            //_errorTextBox.Text = text;
            _visualFeedbackController.TextContainer.SetText("It's ok");
        }
    }

    /// <summary>
    /// Broadcast receiver class which is used to listen to messages from the music service
    /// </summary>
    [BroadcastReceiver]
    [IntentFilter(new[] { Globals.TheAction })]
    public class MusicServiceListener : BroadcastReceiver
    {
        /// <summary>
        /// Method which handles the received intent
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            switch (intent.Action)
            {
                case Globals.TheAction:
                    var text = "It's ok";
                    MainActivity.getInstace().SetButtonText(text);
                    InvokeAbortBroadcast();
                    break;
            }

        }
    }
}