using Android.App;
using Android.Content;
using Android.OS;
using Android.Webkit;
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
        /// Textview object which containes the text
        /// </summary>
        private TextView textView;

        /// <summary>
        /// List items
        /// </summary>
        public List<string> Items { get; set; }

        /// <summary>
        /// Song list adapter
        /// </summary>
        private SongListAdapter songListAdapter;

        /// <summary>
        /// The list view
        /// </summary>
        private ListView listView;

        public Button Play { get; private set; }
        public Button Stop { get; private set; }
        public Button Pause { get; private set; }
        public Button Previous { get; private set; }
        public Button Next { get; private set; }
        public ToggleButton Repeat { get; private set; }
        public ToggleButton Shuffle { get; private set; }

        WebView webView;
        WebSettings webSettings;

        /// <summary>
        /// On create
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Globals.MainLayoutId);

            //Start music service controller
            var firstIntent = new Intent(Globals.ActionEvent.ActionStop);
            firstIntent.SetPackage("DisertationProject.Controller");
            Application.Context.StartService(firstIntent);

            //Start UI interface controller
            var secondIntent = new Intent(Globals.PlayState.Stopped);
            secondIntent.SetPackage("DisertationProject.Controller");
            Application.Context.StartService(secondIntent);

            //webView = FindViewById<WebView>(Resource.Id.webView1);
            //webSettings = webView.Settings;
            //webSettings.JavaScriptEnabled = true;
            //webView.SetWebChromeClient(new WebChromeClient());
            //webView.LoadUrl(@"https://www.youtube.com/embed/gTw2YvutJRA?ecver=2");

            SetupButtons();
            SetupTextContainers();
        }

        /// <summary>
        /// Override OnDestroy method
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        /// <summary>
        /// Override OnPause method
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

        private void SetupButtons()
        {
            Play = Helper.findById(Resource.Id.playButton, FindViewById<Button>);
            Pause = Helper.findById(Resource.Id.pauseButton, FindViewById<Button>);
            Stop = Helper.findById(Resource.Id.stopButton, FindViewById<Button>);
            Previous = Helper.findById(Resource.Id.previousButton, FindViewById<Button>);
            Next = Helper.findById(Resource.Id.nextButton, FindViewById<Button>);
            Repeat = Helper.findById(Resource.Id.repeatButton, FindViewById<ToggleButton>);
            Shuffle = Helper.findById(Resource.Id.shuffleButton, FindViewById<ToggleButton>);

            Play.Click += (sender, args) => SendCommand(Globals.ActionEvent.ActionPlay);
            Pause.Click += (sender, args) => SendCommand(Globals.ActionEvent.ActionPause);
            Stop.Click += (sender, args) => SendCommand(Globals.ActionEvent.ActionStop);
            Previous.Click += (sender, args) => SendCommand(Globals.ActionEvent.ActionPrevious);
            Next.Click += (sender, args) => SendCommand(Globals.ActionEvent.ActionNext);
            Repeat.Click += (sender, args) =>
            {
                if (Repeat.Checked) SendCommand(Globals.ActionEvent.ActionRepeatOn);
                else SendCommand(Globals.ActionEvent.ActionRepeatOff);
            };
            Shuffle.Click += (sender, args) =>
            {
                if (Repeat.Checked) SendCommand(Globals.ActionEvent.ActionShuffleOn);
                else SendCommand(Globals.ActionEvent.ActionShuffleOff);
            };
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void SetupTextContainers()
        {
            textView = Helper.findById(Resource.Id.textView1, FindViewById<TextView>);

            Items = new List<string> { "one", "two", "three" };
            listView = (ListView)FindViewById(Resource.Id.songListView);
            songListAdapter = new SongListAdapter(this, Items);
            //listAdapter = new ArrayAdapter<string>(mainActivity, Android.Resource.Layout.SimpleListItemSingleChoice, Items);
            listView.Adapter = songListAdapter;
            //listAdapter.RegisterDataSetObserver(new MyDataSetObserver());
            //listAdapter.Add("a");
        }


        /// <summary>
        /// Send command
        /// </summary>
        /// <param name="action">The intended ation</param>
        public void SendCommand(string action)
        {
            var intent = new Intent(action);
            Application.Context.StartService(intent);
        }
    }
}
