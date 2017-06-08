using Android.App;
using Android.Content;
using Android.OS;
using DisertationProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Intent
        /// </summary>
        public Intent intent;

        /// <summary>
        /// Current playlist
        /// </summary>
        private Playlist playList;

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
        private DataController dataController;

        /// <summary>
        /// Network controller
        /// </summary>
        private NetworkController networkController;

        /// <summary>
        /// Music player contorller
        /// </summary>
        private MusicPlayerController musicPlayer;

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
            //intent = new Intent(Globals.TestAction);
            networkController = new NetworkController();
            musicPlayer = new MusicPlayerController();
            playList = new Playlist();
            FullPlaylist = new Playlist();
            SetupPlaylist();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            dataController = new DataController();
            //Playlist = new Playlist();
            networkController = new NetworkController();
            musicPlayer = new MusicPlayerController();

            //Make the music player play the next song automatically
            musicPlayer.MediaPlayer.Completion += (sender, args) => Next();
            //When we have error in the media player
            musicPlayer.MediaPlayer.Error += (sender, args) =>
            {
                //StartForeground("Error in media player", "");
                // t.Text = "Error in media player";
                Stop();
            };
        }

        /// <summary>
        /// Setup playlist
        /// </summary>
        private void SetupPlaylist()
        {
            var songList = new List<Song>
            {
                //new Song { Artist = "Alex Jones", Name = "My lovely", Emotion = Globals.Emotions.Happy, Source = Globals.SampleSong2},
                //new Song { Artist = "Tomas Mick", Name = "To you", Emotion = Globals.Emotions.Sad, Source = Globals.SampleSong3},
                new Song { Artist = "Yu", Name = "ERT", Emotion = Globals.Emotions.Sad, Source = Globals.SampleSong4},
                new Song { Artist = "Nicholas", Name = "S1u", Emotion = Globals.Emotions.Angry, Source = Globals.SampleSong5},
                new Song { Artist = "Volt", Name = "is it you", Emotion = Globals.Emotions.Sad, Source = Globals.SampleSong6},
                //new Song { Artist = "Tomas Mick", Name = "To", Emotion = Globals.Emotions.Happy, Source = Globals.SampleSong7},
                //new Song { Artist =  "Mick", Name = "To you", Emotion = Globals.Emotions.Neutral, Source = Globals.SampleSong8},
            };

            FullPlaylist.Add(songList);

            songList = FullPlaylist.SongList.Where(p => p.Emotion == Globals.Emotions.Happy).Select(p => p).ToList();
            HappyPlaylist = new Playlist(songList);

            songList = FullPlaylist.SongList.Where(p => p.Emotion == Globals.Emotions.Neutral).Select(p => p).ToList();
            NeutralPlaylist = new Playlist(songList);

            songList = FullPlaylist.SongList.Where(p => p.Emotion == Globals.Emotions.Sad).Select(p => p).ToList();
            SadPlaylist = new Playlist(songList);

            playList = FullPlaylist;
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
                case Globals.ActionRepeatOn: ToggleRepeat(Globals.State.On); break;
                case Globals.ActionRepeatOff: ToggleRepeat(Globals.State.Off); break;
            }
            //Set sticky as we are a long running operation
            return StartCommandResult.Sticky;
        }

        public void NotifyController(string id)
        {
            var intent = new Intent(id);
            SendBroadcast(intent);
        }

        /// <summary>
        /// Play song method
        /// <param name="songUrl">The song URL string</param>
        /// </summary>
        private void Play()
        {
            if (networkController.IsOnline())
            {
                try
                {
                    NotifyController(Globals.TestAction);
                    musicPlayer.Play(playList.GetCurrentSong().Source);
                    StartForeground("Playing ", playList.GetCurrentSong().Name);
                    networkController.AquireWifiLock();
                }
                catch (Java.Lang.IllegalStateException)
                {
                    NotifyController(Globals.IllegalStateException);
                    // StartForeground("Illegal state exception", "");
                    Stop();
                }
                catch (Exception)
                {
                    SendBroadcast(intent);
                    //StartForeground("General exception", "");
                    Stop();
                }
            }
            else //network is not online
            {
                NotifyController(Globals.NetworkOffline);
                Stop();
                //StartForeground("Check internet connection", "");
                //musicPlayer.Play(playList.GetCurrentSong().Source);
                //StartForeground(playList.GetCurrentSong().ToString());
                //_wifi.AquireWifiLock();
            }
        }

        /// <summary>
        /// Pause action method
        /// </summary>
        private void Pause()
        {
            musicPlayer.Pause();
            StopForeground(true);
        }

        /// <summary>
        /// Stop action method
        /// </summary>
        private void Stop()
        {
            musicPlayer.Stop();
            StopForeground(true);
            networkController.ReleaseWifiLock();
        }

        /// <summary>
        /// Play previous song
        /// </summary>
        private void Previous()
        {
            Stop();
            if (!playList.IsAtBeggining())
            {
                playList.DecrementPosition();
                Play();
            }
            else if (playList.IsRepeatEnabled())
            {
                playList.SetPositionToEnd();
                Play();
            }
        }

        /// <summary>
        /// Play next song
        /// </summary>
        public void Next()
        {
            Stop();
            if (!playList.IsAtEnd())
            {
                playList.IncrementPosition();
                Play();
            }
            else if (playList.IsRepeatEnabled())
            {
                playList.ResetPosition();
                Play();
            }
        }

        /// <summary>
        /// Toggle repeat On or Off
        /// </summary>
        /// <param name="state">State can be "On" / "Off"</param>
        public void ToggleRepeat(Globals.State state)
        {
            playList.Repeat = state;
        }

        /// <summary>
        /// Properly cleanup of your player by releasing resources
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
            if (musicPlayer != null)
            {
                musicPlayer.MediaPlayer.Release();
                musicPlayer = null;
            }
        }

        /// <summary>
        /// Start foreground
        /// When we start on the foreground we will present a notification to the user
        /// When they press the notification it will take them to the main page so they can control the music
        /// </summary>
        private void StartForeground(string firstText, string secondText)
        {
            var pendingIntent = PendingIntent.GetActivity(MainActivity.Context, 0, new Intent(MainActivity.Context, typeof(MainActivity)), PendingIntentFlags.UpdateCurrent);
            var notification = new Notification
            {
                TickerText = new Java.Lang.String(firstText + secondText),
                Icon = Resource.Drawable.ic_stat_av_play_over_video
            };

            notification.Flags |= NotificationFlags.OngoingEvent;
#pragma warning disable CS0618 // Type or member is obsolete
            notification.SetLatestEventInfo(MainActivity.Context, "Music Streaming", firstText + secondText, pendingIntent);
#pragma warning restore CS0618 // Type or member is obsolete
            StartForeground(notificationId, notification);
        }
    }
}