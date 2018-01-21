using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using DisertationProject.Model;
using System;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Music service controller
    /// </summary>
    [Service]
    [IntentFilter(new[] { "ActionPlay",   "ActionPause", "ActionStop",
                          "ActionPrevious","ActionNext",
                          "ActionRepeatOn","ActionRepeatOff" })]
    public class MusicController : Service//, AudioManager.IOnAudioFocusChangeListener
    {
        /// <summary>
        /// Intent
        /// </summary>
        public Intent intent;

        /// <summary>
        /// Data controller
        /// </summary>
        private DataController dataController;

        /// <summary>
        /// Network controller
        /// </summary>
        private NetworkController networkController;

        /// <summary>
        /// Notification Id
        /// </summary>
        private const int notificationId = 1;

        /// <summary>
        /// The media player
        /// </summary>
        private MediaPlayer MediaPlayer;

        /// <summary>
        /// The audio manager
        /// </summary>
        private AudioManager audioManager;

        private string _name = "SongName";

        /// <summary>
        /// State
        /// </summary>
        public PlayState State { get; private set; }

        /// <summary>
        /// On bind
        /// Don't do anything on bind
        /// </summary>
        /// <param name="intent">The intent</param>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent) { throw new NotImplementedException(); }

        /// <summary>
        /// On create simply detect some of our managers
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
            networkController = new NetworkController();
            InitializeMediaPlayer();
        }

        private void InitializeMediaPlayer()
        {
            audioManager = (AudioManager)Application.Context.GetSystemService(AudioService);
            MediaPlayer = new MediaPlayer();

            MediaPlayer.SetAudioStreamType(Stream.Music);

            //Wake mode will be partial to keep the CPU still running under lock screen
            MediaPlayer.SetWakeMode(Application.Context, WakeLockFlags.Partial);

            MediaPlayer.Prepared += (sender, args) => MediaPlayer.Start();

            MediaPlayer.Completion += (sender, args) =>
            {
                Intent intent = new Intent("GetNext");
                intent.PutExtra("GetNext", 0);
                SendBroadcast(intent);
            };

            MediaPlayer.Error += (sender, args) =>
            {
                Stop();
            };
        }

        ///// <summary>
        ///// Setup playlist
        ///// </summary>
        //private void SetupPlaylist()
        //{
        //    var songList = new List<Song>
        //    {
        //        new Song {Id = 101, Artist = "Trumpet", Name = "March", Emotion = Emotions.Happy, Source = Songs.SampleSong1},
        //        new Song {Id = 102, Artist = "Russia", Name = "Katyusha", Emotion = Emotions.Happy, Source = Songs.SampleSong2},
        //        new Song {Id = 103, Artist = "America", Name = "Yankee Doodle Dandy", Emotion = Emotions.Happy, Source = Songs.SampleSong3},
        //        new Song {Id = 104, Artist = "Romania", Name = "National Anthem", Emotion = Emotions.Happy, Source = Songs.SampleSong4}
        //    };

        //    PlayList.Add(songList);

        //    songList = PlayList.SongList.Where(p => p.Emotion == Emotions.Happy).Select(p => p).ToList();
        //    HappyPlaylist = new Playlist(songList);

        //    songList = PlayList.SongList.Where(p => p.Emotion == Emotions.Neutral).Select(p => p).ToList();
        //    NeutralPlaylist = new Playlist(songList);

        //    songList = PlayList.SongList.Where(p => p.Emotion == Emotions.Sad).Select(p => p).ToList();
        //    SadPlaylist = new Playlist(songList);

        //}

        /// <summary>
        /// On start command
        /// </summary>
        /// <param name="intent">The intent</param>
        /// <param name="flags">The start command flags</param>
        /// <param name="startId">Start id</param>
        /// <returns>Start command result</returns>
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var action = Helper.ConvertActionEvent(intent.Action);
            switch (action)
            {
                case ActionEvent.ActionPlay:
                    var source = intent.GetStringExtra("source");
                    var name = intent.GetStringExtra("name");
                    //if (!string.IsNullOrEmpty(source))
                    //{
                    Play(source, name);
                    //}
                    //else
                    //{
                    //    Play();
                    //}
                    break;
                case ActionEvent.ActionStop: Stop(); break;
                case ActionEvent.ActionPause: Pause(); break;
                    //case ActionEvent.ActionPrevious: Previous(); break;
                    //case ActionEvent.ActionNext: Next(); break;
                    //case ActionEvent.ActionRepeatOn: ToggleRepeat(ToggleState.On); break;
                    //case ActionEvent.ActionRepeatOff: ToggleRepeat(ToggleState.Off); break;
            }
            //Set sticky as we are a long running operation
            return StartCommandResult.Sticky;
        }


        /// <summary>
        /// Play song method
        /// <param name="songUrl">The song URL string</param>
        /// </summary>
        //private void Play()
        //{
        //    Play(PlayList.GetCurrentSong().Source);
        //}

        private async void Play(string uri)
        {
            if (!networkController.IsConnected)
            {
                Stop();
                return;
            }
            if (State == PlayState.Paused)
            {
                MediaPlayer.Start();
                State = PlayState.Playing;
                //StartForeground();
                return;
            }

            //if (MediaPlayer.IsPlaying) return;

            try
            {
                MediaPlayer.Reset();
                await MediaPlayer.SetDataSourceAsync(Application.Context, Android.Net.Uri.Parse(uri));
                //var focusResult = audioManager.RequestAudioFocus(this, Stream.Music, AudioFocus.Gain);
                //if (focusResult != AudioFocusRequest.Granted)
                //{
                //    Stop();
                //}
                MediaPlayer.PrepareAsync();
                networkController.AquireWifiLock();
                StartForeground(_name, "T");
                networkController.AquireWifiLock();
            }
            catch (Java.Lang.IllegalStateException ex)
            {
                Stop();
            }
            catch (Exception ex)
            {
                Stop();
            }
        }

        private void Play(string uri, string name)
        {
            _name = name;
            Play(uri);
        }

        /// <summary>
        /// Pause action method
        /// </summary>
        private void Pause()
        {
            if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Pause();
                State = PlayState.Paused;
            }
        }


        /// <summary>
        /// Stop action method
        /// </summary>
        private void Stop()
        {
            if (MediaPlayer.IsPlaying)
                MediaPlayer.Stop();
            MediaPlayer.Reset();
            State = PlayState.Stopped;
            StopForeground(true);
            networkController.ReleaseWifiLock();
        }

        /// <summary>
        /// Get the duration of the current song
        /// </summary>
        /// <returns>The duration of the current playing song</returns>
        private int GetSongDuration()
        {
            var duration = 0;
            try
            {
                duration = MediaPlayer.Duration;
            }
            catch (Exception ex)
            {
                Stop();
            };
            return duration;
        }

        /// <summary>
        /// Get the position of the current song playing
        /// </summary>
        /// <returns>The position of the current playing song</returns>
        public int GetCurrentPosition()
        {
            var position = 0;
            try
            {
                position = MediaPlayer.CurrentPosition;
            }
            catch (Exception ex)
            {
                Stop();
            }
            return position;
        }

        /// <summary>
        /// Properly cleanup of your player by releasing resources
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
            if (MediaPlayer != null)
            {
                MediaPlayer.Release();
                MediaPlayer = null;
            }
        }


        /// <summary>
        /// For a good user experience we should account for when audio focus has changed.
        /// There is only 1 audio output there may be several media services trying to use it so
        /// we should act correctly based on this.  "duck" to be quiet and when we gain go full.
        /// All applications are encouraged to follow this, but are not enforced.
        /// </summary>
        /// <_parameter name="focusChange"></_parameter>
        public void OnAudioFocusChange(AudioFocus focusChange)
        {
            switch (focusChange)
            {
                case AudioFocus.Gain:
                    if (!MediaPlayer.IsPlaying)
                    {
                        MediaPlayer.Start();
                    }

                    MediaPlayer.SetVolume(1.0f, 1.0f);//Turn it up!
                    break;

                case AudioFocus.Loss:
                    //We have lost focus stop!
                    Stop(); break;
                case AudioFocus.LossTransient:
                    //We have lost focus for a short time, but likely to resume so pause
                    Pause(); break;
                case AudioFocus.LossTransientCanDuck:
                    //We have lost focus but should till play at a muted 10% volume
                    if (MediaPlayer.IsPlaying) MediaPlayer.SetVolume(.1f, .1f);//turn it down!
                    break;
            }
        }

        /// <summary>
        /// Start foreground
        /// When we start on the foreground we will present a notification to the user
        /// When they press the notification it will take them to the main page so they can control the music
        /// </summary>
        private void StartForeground(string artist, string song)
        {
            var pendingIntent = PendingIntent.GetActivity(Application.Context, 0, new Intent(Application.Context, typeof(MainActivity)), PendingIntentFlags.UpdateCurrent);
            var text = string.Format("{0} - {1}", artist, song);
            var notification = new Notification
            {
                TickerText = new Java.Lang.String(text),
                Icon = Resource.Drawable.ic_stat_av_play_over_video
            };

            notification.Flags |= NotificationFlags.OngoingEvent;
#pragma warning disable CS0618 // Type or member is obsolete
            notification.SetLatestEventInfo(Application.Context, "Playing", text, pendingIntent);
#pragma warning restore CS0618 // Type or member is obsolete
            StartForeground(notificationId, notification);
        }
    }
}