using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using DisertationProject.Data;
using System.Collections.Generic;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Streaming background service
    /// </summary>
    [Service]
    [IntentFilter(new[] { Globals.ActionPlay, Globals.ActionPause, Globals.ActionStop })]
    public class StreamingBackgroundService : Service
    {
        public List<string> songList;

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
        /// On create simply detect some of our managers
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
            _wifi = new WiFiController();
            _musicPlayer = new MusicPlayerController();

            songList = new List<string>();
            songList.Add(Globals.SampleSong1);
            songList.Add(Globals.SampleSong2);
            songList.Add(Globals.SampleSong3);
        }

        /// <summary>
        /// On bind
        /// Don't do anything on bind
        /// </summary>
        /// <param name="intent">The intent</param>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent) { return null; }

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
                case Globals.ActionPlay: Play(Globals.SampleSong1); break;
                case Globals.ActionStop: Stop(); break;
                case Globals.ActionPause: Pause(); break;
            }
            //Set sticky as we are a long running operation
            return StartCommandResult.Sticky;
        }

        /// <summary>
        /// Play song method
        /// <param name="songUrl">The song URL string</param>
        /// </summary>
        private void Play(string songUrl)
        {
            if (!_musicPlayer.MediaPlayer.IsPlaying)
            {
                _musicPlayer.Play(songUrl);
                StartForeground();
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
        private void StartForeground()
        {
            var pendingIntent = PendingIntent.GetActivity(MainActivity.Context, 0, new Intent(MainActivity.Context, typeof(MainActivity)), PendingIntentFlags.UpdateCurrent);
            var notification = new Notification
            {
                TickerText = new Java.Lang.String("Song started!"),
                Icon = Resource.Drawable.ic_stat_av_play_over_video
            };

            notification.Flags |= NotificationFlags.OngoingEvent;
#pragma warning disable CS0618 // Type or member is obsolete
            notification.SetLatestEventInfo(MainActivity.Context, "Xamarin Streaming", "Playing music!", pendingIntent);
#pragma warning restore CS0618 // Type or member is obsolete
            StartForeground(notificationId, notification);
        }

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