using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using DisertationProject.Data;
using DisertationProject.Data.Models;
using DisertationProject.Model;
using System.Collections.Generic;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Streaming background service
    /// </summary>
    [Service]
    [IntentFilter(new[] { Globals.ActionPlay, Globals.ActionPause, Globals.ActionStop, Globals.ActionPrevious, Globals.ActionNext,
                          Globals.ActionRepeatOn, Globals.ActionRepeatOff })]
    public class MusicServiceController : Service
    {
        /// <summary>
        /// Song list
        /// </summary>
        public Playlist playlist;

        /// <summary>
        /// Wifi controller
        /// </summary>
        private WiFiController _wifi;

        /// <summary>
        /// Music player contorller
        /// </summary>
        private MusicPlayerController _musicPlayer;

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
            _wifi = new WiFiController();
            _musicPlayer = new MusicPlayerController();
            playlist = new Playlist();
            SetupPlaylist();
        }

        /// <summary>
        /// Setup playlist
        /// </summary>
        private void SetupPlaylist()
        {
            playlist.Add(new List<Song>{
                new Song{Source = Globals.SampleSong2, SongName = "Song 1"}, 
                new Song{Source = Globals.SampleSong3, SongName = "Song 2"},
                new Song{Source = Globals.SampleSong4, SongName = "Song 3"},
                new Song{Source = Globals.SampleSong5, SongName = "Song 4"},
                new Song{Source = Globals.SampleSong6, SongName = "Song 5"},
                new Song{Source = Globals.SampleSong7, SongName = "Song 6"},
                new Song{Source = Globals.SampleSong8, SongName = "Song 7"},
                new Song{Source = Globals.SampleSong9, SongName = "Song 8"}
            });
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
            if (!_musicPlayer.MediaPlayer.IsPlaying)
            {
                _musicPlayer.Play(playlist.GetCurrentItem().Source);
                StartForeground(playlist.GetCurrentItem().SongName);
                _wifi.AquireWifiLock();
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
            _wifi.ReleaseWifiLock();
        }

        /// <summary>
        /// Play previous song
        /// </summary>
        private void Previous()
        {
            Stop();
            if (!playlist.IsAtBeggining())
            {
                playlist.DecrementPosition();
                Play();
            }
            else if (playlist.IsRepeatOn())
            {
                playlist.SetPositionToEnd();
                Play();
            }
        }

        /// <summary>
        /// Play next song
        /// </summary>
        public void Next()
        {
            Stop();
            if (!playlist.IsAtEnd())
            {
                playlist.IncrementPosition();
                Play();
            }
            else if (playlist.IsRepeatOn())
            {
                playlist.ResetPosition();
                Play();
            }
        }

        /// <summary>
        /// Toggle repeat on
        /// </summary>
        public void ToggleRepeatOn()
        {
            playlist.SetRepeatOn();
        }

        /// <summary>
        /// Toggle repeat off
        /// </summary>
        public void ToggleRepeatOff()
        {
            playlist.SetRepeatOff();
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
        private void StartForeground(string text)
        {
            var pendingIntent = PendingIntent.GetActivity(MainController.Context, 0, new Intent(MainController.Context, typeof(MainController)), PendingIntentFlags.UpdateCurrent);
            var notification = new Notification
            {
                TickerText = new Java.Lang.String("Playing "+text),
                Icon = Resource.Drawable.ic_stat_av_play_over_video
            };

            notification.Flags |= NotificationFlags.OngoingEvent;
#pragma warning disable CS0618 // Type or member is obsolete
            notification.SetLatestEventInfo(MainController.Context, "Music Streaming", "Playing "+text, pendingIntent);
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