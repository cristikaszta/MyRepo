using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using DisertationProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Music service controller
    /// </summary>
    [Service]
    [IntentFilter(new[] { Globals.ActionPlay, Globals.ActionPause, Globals.ActionStop, Globals.ActionPrevious, Globals.ActionNext,
                          Globals.ActionRepeatOn, Globals.ActionRepeatOff })]
    public class MusicServiceController : Service
    {
        /// <summary>
        /// Current play list
        /// </summary>
        private Playlist _playList;

        /// <summary>
        /// Full playlist
        /// </summary>
        public Playlist FullPlaylist;

        /// <summary>
        /// Happy playlist
        /// </summary>
        public Playlist HappyPlaylist;

        /// <summary>
        /// Sad playlist
        /// </summary>
        public Playlist SadPlaylist;

        /// <summary>
        /// Neutral playlist
        /// </summary>
        public Playlist NeutralPlaylist;

        /// <summary>
        /// Data controller
        /// </summary>
        private DataController _dataController;

        /// <summary>
        /// Network controller
        /// </summary>
        private NetworkController _networkController;

        /// <summary>
        /// Music player contorller
        /// </summary>
        private MusicPlayerController _musicPlayer;

        /// <summary>
        /// Database controller
        /// </summary>
        private DatabaseController _databaseController;

        /// <summary>
        /// Notification Id
        /// </summary>
        private const int notificationId = 1;

        /// <summary>
        /// On bind
        /// Don't do anything on bind
        /// </summary>
        /// <param name="intent">The intent</param>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent) { return null; }

        /// <summary>
        /// On create simply detect some of our managers
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
            _databaseController = new DatabaseController();
            _networkController = new NetworkController();
            _musicPlayer = new MusicPlayerController();
            _playList = new Playlist();
            FullPlaylist = new Playlist();
            SetupPlaylist();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            _dataController = new DataController();
            //Playlist = new Playlist();
            _networkController = new NetworkController();
            _musicPlayer = new MusicPlayerController();

            //Make the music player play the next song automatically
            _musicPlayer.MediaPlayer.Completion += (sender, args) => Next();
            //When we have error in the media player
            _musicPlayer.MediaPlayer.Error += (sender, args) =>
            {
                StartForeground("Error in media player", "");
                Stop();
            };
        }

        /// <summary>
        /// Setup playlist
        /// </summary>
        private void SetupPlaylist()
        {
            _dataController.GetSongs();

            var songList = _databaseController.GetSongs();
            FullPlaylist.Add(songList);

            songList = FullPlaylist.SongList.Where(p => p.Type == "H").Select(p => p).ToList();
            HappyPlaylist = new Playlist(songList);

            songList = FullPlaylist.SongList.Where(p => p.Type == "N").Select(p => p).ToList();
            NeutralPlaylist = new Playlist(songList);

            songList = FullPlaylist.SongList.Where(p => p.Type == "S").Select(p => p).ToList();
            SadPlaylist = new Playlist(songList);

            _playList = SadPlaylist;
        }

        /// <summary>
        /// On start command
        /// </summary>
        /// <param name="intent">The intent</param>
        /// <param name="flags">The start command flags</param>
        /// <param name="startId">Start id</param>
        /// <returns>Start command result</returns>
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case Globals.ActionPlay: Play(); break;
                case Globals.ActionStop: Stop(); break;
                case Globals.ActionPause: Pause(); break;
                case Globals.ActionPrevious: Previous(); break;
                case Globals.ActionNext: Next(); break;
                case Globals.ActionRepeatOn: ToggleRepeatOn(); break;
                case Globals.ActionRepeatOff: ToggleRepeatOff(); break;
            }
            //Set sticky as we are a long running operation
            return StartCommandResult.Sticky;
        }

        /// <summary>
        /// Play song method
        /// <param name="songUrl">The song URL string</param>
        /// </summary>
        private void Play()
        {
            if (_networkController.IsOnline())
            {
                try
                {
                    _musicPlayer.Play(_playList.GetCurrentItem().Source);
                    StartForeground("Playing ", _playList.GetCurrentItem().Name);
                    _networkController.AquireWifiLock();
                }
                catch (Java.Lang.IllegalStateException)
                {
                    StartForeground("Illegal state exception", "");
                    Stop();
                }
                catch (Exception)
                {
                    StartForeground("General exception", "");
                    Stop();
                }
            }
            else
            {
                Stop();
                StartForeground("Check internet connection", "");
                //_musicPlayer.Play(_playList.GetCurrentItem().Source);
                //StartForeground(_playList.GetCurrentItem().ToString());
                //_wifi.AquireWifiLock();
            }
        }

        /// <summary>
        /// Pause action method
        /// </summary>
        private void Pause()
        {
            _musicPlayer.Pause();
            StopForeground(true);
        }

        /// <summary>
        /// Stop action method
        /// </summary>
        private void Stop()
        {
            _musicPlayer.Stop();
            StopForeground(true);
            _networkController.ReleaseWifiLock();
        }

        /// <summary>
        /// Play previous song
        /// </summary>
        private void Previous()
        {
            Stop();
            if (!_playList.IsAtBeggining())
            {
                _playList.DecrementPosition();
                Play();
            }
            else if (_playList.IsRepeatEnabled())
            {
                _playList.SetPositionToEnd();
                Play();
            }
        }

        /// <summary>
        /// Play next song
        /// </summary>
        public void Next()
        {
            Stop();
            if (!_playList.IsAtEnd())
            {
                _playList.IncrementPosition();
                Play();
            }
            else if (_playList.IsRepeatEnabled())
            {
                _playList.ResetPosition();
                Play();
            }
        }

        /// <summary>
        /// Toggle repeat on
        /// </summary>
        public void ToggleRepeatOn()
        {
            _playList.SetRepeatOn();
        }

        /// <summary>
        /// Toggle repeat off
        /// </summary>
        public void ToggleRepeatOff()
        {
            _playList.SetRepeatOff();
        }

        /// <summary>
        /// Properly cleanup of your player by releasing resources
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
            if (_musicPlayer != null)
            {
                _musicPlayer.MediaPlayer.Release();
                _musicPlayer = null;
            }
        }

        /// <summary>
        /// Start foreground
        /// When we start on the foreground we will present a notification to the user
        /// When they press the notification it will take them to the main page so they can control the music
        /// </summary>
        private void StartForeground(string firstText, string secondText)
        {
            var pendingIntent = PendingIntent.GetActivity(MainController.Context, 0, new Intent(MainController.Context, typeof(MainController)), PendingIntentFlags.UpdateCurrent);
            var notification = new Notification
            {
                TickerText = new Java.Lang.String(firstText + secondText),
                Icon = Resource.Drawable.ic_stat_av_play_over_video
            };

            notification.Flags |= NotificationFlags.OngoingEvent;
#pragma warning disable CS0618 // Type or member is obsolete
            notification.SetLatestEventInfo(MainController.Context, "Music Streaming", firstText + secondText, pendingIntent);
#pragma warning restore CS0618 // Type or member is obsolete
            StartForeground(notificationId, notification);
        }

        /// <summary>
        /// Music broadcast receiver
        /// Handle AudioBecomingNoisy aka headphones unplugged
        /// </summary>
        [BroadcastReceiver]
        [IntentFilter(new[] { AudioManager.ActionAudioBecomingNoisy })]
        public class MusicBroadcastReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (intent.Action != AudioManager.ActionAudioBecomingNoisy)
                    return;
                //signal the service to stop!
                var stopIntent = new Intent(Globals.ActionStop);
                context.StartService(stopIntent);
            }
        }
    }
}