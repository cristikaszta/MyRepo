using Android.App;
using Android.Widget;
using DisertationProject.Model;
using System.Collections.Generic;
using Android.Content;
using Android.OS;

namespace DisertationProject.Controller
{
    [Service]
    [IntentFilter(new[] { Globals.Errors.NetworkOffline, Globals.Errors.IllegalStateException,
                          Globals.PlayState.Playing, Globals.PlayState.Paused, Globals.PlayState.Stopped })]
    public class UserInterfaceController : Service
    {
        /// <summary>
        /// The main activity
        /// </summary>
        private MainActivity mainActivity;

        /// <summary>
        /// Text to be displayed
        /// </summary>
        public string TextBox
        {
            get
            {
                return textView.Text;
            }
            set
            {
                textView.Text = value;
            }
        }


        /// <summary>
        /// Notification Id
        /// </summary>
        private const int notificationId = 2;

        /// <summary>
        /// Textview object which containes the text
        /// </summary>
        private TextView textView;

        /// <summary>
        /// List items
        /// </summary>
        public List<string> Items { get; set; }

        /// <summary>
        /// The list view
        /// </summary>
        private ListView listView;

        /// <summary>
        /// List adapter
        /// </summary>
        //private ArrayAdapter<string> listAdapter;

        /// <summary>
        /// Song list adapter
        /// </summary>
        private SongListAdapter songListAdapter;

        public Button Play { get; private set; }
        public Button Stop { get; private set; }
        public Button Pause { get; private set; }
        public Button Previous { get; private set; }
        public Button Next { get; private set; }
        public ToggleButton Repeat { get; private set; }
        public ToggleButton Shuffle { get; private set; }

        public override void OnCreate()
        {
            base.OnCreate();
            mainActivity = MainActivity.getInstace();
            SetupButtons();
            SetupTextContainers();
            StartForeground("title", "artist", "song");
        }

        /// <summary>
        /// Handle the intents that come in
        /// </summary>
        /// <param name="intent">Intent. Can be play action or error from player</param>
        /// <param name="flags">Flags</param>
        /// <param name="startId">Id</param>
        /// <returns></returns>
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case Globals.PlayState.Playing:
                case Globals.PlayState.Paused:
                case Globals.PlayState.Stopped:
                case Globals.Errors.DatabaseError:
                case Globals.Errors.NetworkOffline:
                case Globals.Errors.IllegalStateException:
                case Globals.Errors.GeneralException:
                    TextBox = intent.Action;
                    break;
            }
            //Set sticky as we are a long running operation
            return StartCommandResult.Sticky;
        }

        private void SetupButtons()
        {
            Play = Helper.findById(Resource.Id.playButton, mainActivity.FindViewById<Button>);
            Pause = Helper.findById(Resource.Id.pauseButton, mainActivity.FindViewById<Button>);
            Stop = Helper.findById(Resource.Id.stopButton, mainActivity.FindViewById<Button>);
            Previous = Helper.findById(Resource.Id.previousButton, mainActivity.FindViewById<Button>);
            Next = Helper.findById(Resource.Id.nextButton, mainActivity.FindViewById<Button>);
            Repeat = Helper.findById(Resource.Id.repeatButton, mainActivity.FindViewById<ToggleButton>);
            Shuffle = Helper.findById(Resource.Id.shuffleButton, mainActivity.FindViewById<ToggleButton>);

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
        /// Send command
        /// </summary>
        /// <param name="action">The intended ation</param>
        public void SendCommand(string action)
        {
            var intent = new Intent(action);
            Application.Context.StartService(intent);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void SetupTextContainers()
        {
            textView = Helper.findById(Resource.Id.textView1, mainActivity.FindViewById<TextView>);

            Items = new List<string> { "one", "two", "three" };
            listView = (ListView)mainActivity.FindViewById(Resource.Id.songListView);
            songListAdapter = new SongListAdapter(mainActivity, Items);
            //listAdapter = new ArrayAdapter<string>(mainActivity, Android.Resource.Layout.SimpleListItemSingleChoice, Items);
            listView.Adapter = songListAdapter;
            //listAdapter.RegisterDataSetObserver(new MyDataSetObserver());
            //listAdapter.Add("a");
        }

        public void RefreshList()
        {
            //Items.Add("C");
            //listAdapter.Clear();
            //listAdapter.Add("B");
            //listAdapter.NotifyDataSetChanged();
            //_listView.RefreshDrawableState();
        }

        /// <summary>
        /// On bind
        /// Don't do anything on bind
        /// </summary>
        /// <param name="intent">The intent</param>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent) { return null; }

        private void StartForeground(string title, string artist, string song)
        {
            var pendingIntent = PendingIntent.GetActivity(Application.Context, 0, new Intent(Application.Context, typeof(MainActivity)), PendingIntentFlags.UpdateCurrent);
            var notification = new Notification
            {
                TickerText = new Java.Lang.String(artist + song),
                Icon = Resource.Drawable.ic_stat_av_play_over_video
            };

            notification.Flags |= NotificationFlags.OngoingEvent;
#pragma warning disable CS0618 // Type or member is obsolete
            notification.SetLatestEventInfo(Application.Context, title, string.Format("{0} - {1}", artist, song), pendingIntent);
#pragma warning restore CS0618 // Type or member is obsolete
            StartForeground(notificationId, notification);
        }
    }
}