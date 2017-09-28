using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using DisertationProject.Model;
using System;
using System.Collections.Generic;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = Globals.PROJECT_LABEL, MainLauncher = true, Icon = "@drawable/ic_launcher")]

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

        SongComletionReceiver _songCompletionReceiver;

        /// <summary>
        /// On create
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Globals.MainLayoutId);
            SetupButtons();
            SetupPlaylist();
            SetupTextContainers();

            _songCompletionReceiver = new SongComletionReceiver();

            _songCompletionReceiver.SongCompletionEventHandler += (sender, args) =>
            {
                if (!PlayList.IsAtEnd)
                {
                    PlayList.IncrementPosition();
                }
                else if (PlayList.Repeat == ToggleState.On)
                {
                    PlayList.ResetPosition();
                }
                else
                {
                    return;
                }
                PlayList.IncrementPosition();
                var source = PlayList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionPlay, source);
            };

            RegisterReceiver(_songCompletionReceiver, new IntentFilter("GetNext"));
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
                var currentPosition = args.Position;
                PlayList.Position = currentPosition;
                var name = PlayList.GetCurrentSong().Name;
                var source = PlayList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionPlay, source, name);
            };
        }

        private void SetupPlaylist()
        {
            PlayList = new Playlist();
            var songList = new List<Song>
            {
                new Song { Artist = "Trumpet", Name = "March", Emotion = Emotion.Happy, Source = Globals.Songs.SampleSong1},
                new Song { Artist = "Russia", Name = "Katyusha", Emotion = Emotion.Happy, Source = Globals.Songs.SampleSong2},
                new Song { Artist = "America", Name = "Yankee Doodle Dandy", Emotion = Emotion.Happy, Source = Globals.Songs.SampleSong3},
                new Song { Artist = "Romania", Name = "National Anthem", Emotion = Emotion.Happy, Source = Globals.Songs.SampleSong4}
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

        public void SendCommand(ActionEvent action, string source, string name)
        {
            var stringifiedAction = Helper.ConvertActionEvent(action);
            var intent = new Intent(stringifiedAction, null, this, typeof(MusicController));
            intent.PutExtra("source", source);
            intent.PutExtra("name", name);
            StartService(intent);
        }


    }

    [IntentFilter(new[] { "GetNext" })]
    public class SongComletionReceiver : BroadcastReceiver
    {
        public event EventHandler SongCompletionEventHandler;

        public override void OnReceive(Context context, Intent intent)
        {
            OnSomeDataReceived(new EventArgs());
        }

        protected virtual void OnSomeDataReceived(EventArgs e)
        {
            SongCompletionEventHandler?.Invoke(this, e);
        }
    }
}
