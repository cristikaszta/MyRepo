using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using DisertationProject.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Song = DisertationProject.Data.Models.Song;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = Globals.ProjectLabel, MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class MainController : Activity
    {
        SqlConnection sqlconn;
        string connsqlstring = string.Format("Server=tcp:ourserver.database.windows.net,1433;Data Source=ourserver.database.windows.net;Initial Catalog=ourdatabase;Persist Security Info=False;User ID=lanister;Password=tyrion0!;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;");

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
            try
            {
                sqlconn = new SqlConnection(connsqlstring);

                sqlconn.Open();
            }
            catch (Exception e)
            {

            }

            Context = ApplicationContext;
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
            //Long click to test the get song by id
            _buttons[Globals.PlayButtonId].LongClick += (sender, args) => MainActivity_LongClick(16);
        }

        private void MainActivity_LongClick(int playListId)
        {
            GetSong(playListId);
        }

        public void GetSong(int playListId)
        {
            var song = new Song();

            SqlDataReader reader;
            SqlCommand command = sqlconn.CreateCommand();
            command.CommandText = "SELECT * FROM Songs WHERE Id = @playListId";

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@playListId";
            param.Value = playListId;

            command.Parameters.Add(param);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    song.Id = int.Parse(reader.GetSqlValue(0).ToString());
                    song.Name = reader.GetSqlValue(1).ToString();
                    song.Source = reader.GetSqlValue(2).ToString();
                    song.Artist = reader.GetSqlValue(3).ToString();
                    song.Type = reader.GetSqlValue(4).ToString();
                }

            }
            reader.Close();
            // sqlconn.Close();


            //WORK IN PROGRESS TO PLAY VIDEO FROM YOUTUBE LINK (HARCODED FOR NOW)
            //I THINK IT DOES NOT WORK IN MY EMULATOR , PLEASE TRY DIRECTLY IN THE PHONE

            //Obtain a reference to the VideoView in code.
            var videoView = FindViewById<VideoView>(Resource.Id.YoutubeVideoView);
            // Create an Android.Net.Uri for the video.
            var uri = Android.Net.Uri.Parse("rtsp://r2---sn-a5m7zu76.c.youtube.com/Ck0LENy73wIaRAnTmlo5oUgpQhMYESARFEgGUg5yZWNvbW1lbmRhdGlvbnIhAWL2kyn64K6aQtkZVJdTxRoO88HsQjpE1a8d1GxQnGDmDA==/0/0/0/video.3gp");
            // Pass this Uri to the VideoView.
            videoView.SetVideoURI(uri);
            // Start the VideoView.
            videoView.RequestFocus();
            videoView.Start();
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