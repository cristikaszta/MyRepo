using Android.App;
using Android.Content;
using Android.Media;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using DisertationProject.Data;
using System;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Music player controller
    /// </summary>
    //[Service]
    //[IntentFilter(new[] { Globals.ActionPlay, Globals.ActionPause, Globals.ActionStop })]
    public class MusicPlayerController : Service, AudioManager.IOnAudioFocusChangeListener
    {
        public MediaPlayer mediaPlayer;
        public AudioManager audioManager;
        private bool isPaused;
        private const int notificationId = 1;

        /// <summary>
        /// On create simply detect some of our managers
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
            //Find our audio and notificaton managers
            audioManager = (AudioManager)GetSystemService(AudioService);
            //wifiManager = (WifiManager)GetSystemService(WifiService);
        }

        /// <summary>
        /// On bind
        /// Don't do anything on bind
        /// </summary>
        /// <param name="intent">The intent</param>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent) { return null; }

        ///// <summary>
        ///// On start command
        ///// </summary>
        ///// <param name="intent">The intent</param>
        ///// <param name="flags">The start command flags</param>
        ///// <param name="startId">Start id</param>
        ///// <returns>Start command result</returns>
        //public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        //{
        //    switch (intent.Action)
        //    {
        //        case Globals.ActionPlay: Play(); break;
        //        case Globals.ActionStop: Stop(); break;
        //        case Globals.ActionPause: Pause(); break;
        //    }
        //    //Set sticky as we are a long running operation
        //    return StartCommandResult.Sticky;
        //}

        /// <summary>
        /// Initialize player
        /// </summary>
        private void IntializePlayer()
        {
            mediaPlayer = new MediaPlayer();

            //Tell our player to stream music
            mediaPlayer.SetAudioStreamType(Stream.Music);

            //Wake mode will be partial to keep the CPU still running under lock screen
            mediaPlayer.SetWakeMode(ApplicationContext, WakeLockFlags.Partial);

            //When we have prepared the song start playback
            mediaPlayer.Prepared += (sender, args) => mediaPlayer.Start();

            //When we have reached the end of the song stop ourselves, however you could signal next track here.
            mediaPlayer.Completion += (sender, args) => Stop();

            mediaPlayer.Error += (sender, args) =>
            {
                //playback error
                Console.WriteLine("Error in playback resetting: " + args.What);
                Stop();//this will clean up and reset properly.
            };
        }

        /// <summary>
        /// Play song method
        /// </summary>
        public async void Play()
        {
            if (isPaused && mediaPlayer != null)
            {
                isPaused = false;
                //We are simply paused so just start again
                mediaPlayer.Start();
                StartForeground();
                return;
            }

            if (mediaPlayer == null) { IntializePlayer(); }
            if (mediaPlayer.IsPlaying) return;

            try
            {
                await mediaPlayer.SetDataSourceAsync(ApplicationContext, Android.Net.Uri.Parse(Globals.SampleSong));
                var focusResult = audioManager.RequestAudioFocus(this, Stream.Music, AudioFocus.Gain);
                if (focusResult != AudioFocusRequest.Granted)
                {
                    //could not get audio focus
                    Console.WriteLine("Could not get audio focus");
                }
                mediaPlayer.PrepareAsync();
                AquireWifiLock();
                StartForeground();
            }
            catch (Exception ex)
            {
                //unable to start playback log error
                Console.WriteLine("Unable to start playback: " + ex);
            }
        }

        /// <summary>
        /// Start foreground
        /// When we start on the foreground we will present a notification to the user
        /// When they press the notification it will take them to the main page so they can control the music
        /// </summary>
        private void StartForeground()
        {
            var pendingIntent = PendingIntent.GetActivity(ApplicationContext, 0, new Intent(ApplicationContext, typeof(MainActivity)), PendingIntentFlags.UpdateCurrent);
            var notification = new Notification
            {
                TickerText = new Java.Lang.String("Song started!"),
                Icon = Resource.Drawable.ic_stat_av_play_over_video
            };

            notification.Flags |= NotificationFlags.OngoingEvent;
#pragma warning disable CS0618 // Type or member is obsolete
            notification.SetLatestEventInfo(ApplicationContext, "Xamarin Streaming", "Playing music!", pendingIntent);
#pragma warning restore CS0618 // Type or member is obsolete
            StartForeground(notificationId, notification);
        }

        /// <summary>
        /// Pause song method
        /// </summary>
        public void Pause()
        {
            if (mediaPlayer == null)
                return;
            if (mediaPlayer.IsPlaying)
                mediaPlayer.Pause();
            StopForeground(true);
            isPaused = true;
        }

        /// <summary>
        /// Stop song method
        /// </summary>
        public void Stop()
        {
            if (mediaPlayer == null)
                return;
            if (mediaPlayer.IsPlaying)
                mediaPlayer.Stop();
            mediaPlayer.Reset();
            isPaused = false;
            StopForeground(true);
            _wifi.ReleaseWifiLock();
        }

        ///// <summary>
        ///// Lock the wifi so we can still stream under lock screen
        ///// </summary>
        //private void AquireWifiLock()
        //{
        //    if (wifiLock == null) { wifiLock = wifiManager.CreateWifiLock(WifiMode.Full, "xamarin_wifi_lock"); }
        //    wifiLock.Acquire();
        //}

        ///// <summary>
        ///// This will release the wifi lock if it is no longer needed
        ///// </summary>
        //private void ReleaseWifiLock()
        //{
        //    if (wifiLock == null)
        //        return;

        //    wifiLock.Release();
        //    wifiLock = null;
        //}

        /// <summary>
        /// Properly cleanup of your player by releasing resources
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
            if (mediaPlayer != null)
            {
                mediaPlayer.Release();
                mediaPlayer = null;
            }
        }

        /// <summary>
        /// For a good user experience we should account for when audio focus has changed.
        /// There is only 1 audio output there may be several media services trying to use it so
        /// we should act correctly based on this.  "duck" to be quiet and when we gain go full.
        /// All applications are encouraged to follow this, but are not enforced.
        /// </summary>
        /// <param name="focusChange"></param>
        public void OnAudioFocusChange(AudioFocus focusChange)
        {
            switch (focusChange)
            {
                case AudioFocus.Gain:
                    if (mediaPlayer == null) IntializePlayer();

                    if (!mediaPlayer.IsPlaying)
                    {
                        mediaPlayer.Start();
                        isPaused = false;
                    }

                    mediaPlayer.SetVolume(1.0f, 1.0f);//Turn it up!
                    break;

                case AudioFocus.Loss:
                    //We have lost focus stop!
                    Stop(); break;
                case AudioFocus.LossTransient:
                    //We have lost focus for a short time, but likely to resume so pause
                    Pause(); break;
                case AudioFocus.LossTransientCanDuck:
                    //We have lost focus but should till play at a muted 10% volume
                    if (mediaPlayer.IsPlaying) mediaPlayer.SetVolume(.1f, .1f);//turn it down!
                    break;
            }
        }
    }
}