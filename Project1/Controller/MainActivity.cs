using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using DisertationProject.Model;
using System;
using System.Collections.Generic;
using static DisertationProject.Model.Globals;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = PROJECT_LABEL, MainLauncher = true, Icon = "@drawable/ic_launcher")]

    public class MainActivity : Activity
    {

        /// <summary>
        /// Textview object which containes the text
        /// </summary>
        private TextView textView;

        /// <summary>
        /// List items
        /// </summary>
        //public List<Song> Items { get; set; }

        /// <summary>
        /// Song list adapter
        /// </summary>
        private SongListAdapter songListAdapter;

        /// <summary>
        /// The list view
        /// </summary>
        private ListView listView;

        /// <summary>
        /// Playlist
        /// </summary>
        public Playlist PlayList { get; set; }

        public Button Play { get; private set; }
        public Button Stop { get; private set; }
        public Button Pause { get; private set; }
        public Button Previous { get; private set; }
        public Button Next { get; private set; }
        public ToggleButton Repeat { get; private set; }
        public ToggleButton Shuffle { get; private set; }

        public delegate string GetNextSong();

        SomeBroadcastReceiver _broadcastReceiver;

        /// <summary>
        /// On create
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(MainLayoutId);
            SetupButtons();
            SetupPlaylist();
            SetupTextContainers();

            _broadcastReceiver = new SomeBroadcastReceiver();

            _broadcastReceiver.SomeDataReceived += (sender, e) =>
            {
                SendCommand(ActionEvent.ActionPlay, PlayList.SongList[1].Source);
            };

            RegisterReceiver(_broadcastReceiver, new IntentFilter("GetNext"));
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

            Play.Click += (sender, args) => SendCommand(ActionEvent.ActionPlay);
            Pause.Click += (sender, args) => SendCommand(ActionEvent.ActionPause);
            Stop.Click += (sender, args) => SendCommand(ActionEvent.ActionStop);
            Previous.Click += (sender, args) => SendCommand(ActionEvent.ActionPrevious);
            Next.Click += (sender, args) => SendCommand(ActionEvent.ActionNext);
            Repeat.Click += (sender, args) =>
            {
                if (Repeat.Checked)
                    SendCommand(ActionEvent.ActionRepeatOn);
                else SendCommand(ActionEvent.ActionRepeatOff);
            };
            Shuffle.Click += (sender, args) =>
            {
                if (Repeat.Checked) SendCommand(ActionEvent.ActionShuffleOn);
                else SendCommand(ActionEvent.ActionShuffleOff);
            };
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void SetupTextContainers()
        {
            textView = Helper.findById(Resource.Id.textView1, FindViewById<TextView>);
            listView = (ListView)FindViewById(Resource.Id.songListView);
            songListAdapter = new SongListAdapter(this, PlayList.SongList);
            listView.Adapter = songListAdapter;
            listView.ItemClick += (sender, args) =>
            {
                var source = PlayList.SongList[(int)args.Id].Source;
                SendCommand(ActionEvent.ActionPlay, source);
            };
        }

        private void SetupPlaylist()
        {
            PlayList = new Playlist();
            var songList = new List<Song>
            {
                new Song {Id = 0, Artist = "Trumpet", Name = "March", Emotion = Emotions.Happy, Source = Songs.SampleSong1},
                new Song {Id = 1, Artist = "Russia", Name = "Katyusha", Emotion = Emotions.Happy, Source = Songs.SampleSong2},
                new Song {Id = 2, Artist = "America", Name = "Yankee Doodle Dandy", Emotion = Emotions.Happy, Source = Songs.SampleSong3},
                new Song {Id = 3, Artist = "Romania", Name = "National Anthem", Emotion = Emotions.Happy, Source = Songs.SampleSong4}
            };

            PlayList.Add(songList);
        }


        /// <summary>
        /// Send command
        /// </summary>
        /// <param name="action">The intended ation</param>
        public void SendCommand(ActionEvent action)
        {
            var stringifiedAction = Helper.ConvertActionEvent(action);
            var intent = new Intent(stringifiedAction, null, this, typeof(MusicController));
            StartService(intent);
        }

        public void SendCommand(ActionEvent action, string source)
        {
            var stringifiedAction = Helper.ConvertActionEvent(action);
            var intent = new Intent(stringifiedAction, null, this, typeof(MusicController));
            intent.PutExtra("source", source);
            StartService(intent);
        }


    }

    [IntentFilter(new[] { "GetNext" })]
    public class SomeBroadcastReceiver : BroadcastReceiver
    {
        public event EventHandler SomeDataReceived;

        public override void OnReceive(Context context, Intent intent)
        {
            OnSomeDataReceived(new EventArgs());
        }

        protected virtual void OnSomeDataReceived(EventArgs e)
        {
            SomeDataReceived?.Invoke(this, e);
        }
    }
}
