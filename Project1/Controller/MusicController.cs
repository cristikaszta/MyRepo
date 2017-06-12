using Android.App;
using Android.Content;
using Android.Media;
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
    [IntentFilter(new[] { Globals.ActionEvent.ActionPlay,     Globals.ActionEvent.ActionPause, Globals.ActionEvent.ActionStop,
                          Globals.ActionEvent.ActionPrevious, Globals.ActionEvent.ActionNext,
                          Globals.ActionEvent.ActionRepeatOn, Globals.ActionEvent.ActionRepeatOff })]
    public class MusicController : Service, AudioManager.IOnAudioFocusChangeListener
    {
        /// <summary>
        /// Intent
        /// </summary>
        public Intent intent;

        /// <summary>
        /// Playlist
        /// </summary>
        public Playlist PlayList { get; set; }

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

        /// <summary>
        /// State
        /// </summary>
        public string State { get; private set; }

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
            networkController = new NetworkController();
            //dataController = new DataController();
            InitializeMediaPlayer();
            PlayList = new Playlist();
            SetupPlaylist();
        }

        private void InitializeMediaPlayer()
        {
            audioManager = (AudioManager)Application.Context.GetSystemService(AudioService);
            MediaPlayer = new MediaPlayer();

            //Tell our player to stream music
            MediaPlayer.SetAudioStreamType(Stream.Music);

            //Wake mode will be partial to keep the CPU still running under lock screen
            MediaPlayer.SetWakeMode(Application.Context, WakeLockFlags.Partial);

            //When we have prepared the song start playback
            MediaPlayer.Prepared += (sender, args) => MediaPlayer.Start();

            //When we have reached the end of the song stop ourselves, however you could signal next track here.
            MediaPlayer.Completion += (sender, args) => Next();

            MediaPlayer.Error += (sender, args) =>
            {
                Stop();//this will clean up and reset properly.
                throw new Exception("Error in playback resetting: ");
            };
        }

        /// <summary>
        /// Initialize
        /// </summary>
        //private void Initialize()
        //{
        //    dataController = new DataController();
        //    //Playlist = new Playlist();
        //    networkController = new NetworkController();
        //    //musicPlayer = new MusicPLayer();

        //    //Make the music player play the next song automatically
        //    musicPlayer.MediaPlayer.Completion += (sender, args) => Next();
        //    //When we have error in the media player
        //    musicPlayer.MediaPlayer.Error += (sender, args) =>
        //    {
        //        //StartForeground("Error in media player", "");
        //        // t.Text = "Error in media player";
        //        Stop();
        //    };
        //}

        /// <summary>
        /// Setup playlist
        /// </summary>
        private void SetupPlaylist()
        {
            var songList = new List<Song>
            {
                new Song { Artist = "Artist1", Name = "SongName", Emotion = Globals.Emotions.Happy, Source = Globals.Songs.SampleSong1},
            };

            PlayList.Add(songList);

            songList = PlayList.SongList.Where(p => p.Emotion == Globals.Emotions.Happy).Select(p => p).ToList();
            HappyPlaylist = new Playlist(songList);

            songList = PlayList.SongList.Where(p => p.Emotion == Globals.Emotions.Neutral).Select(p => p).ToList();
            NeutralPlaylist = new Playlist(songList);

            songList = PlayList.SongList.Where(p => p.Emotion == Globals.Emotions.Sad).Select(p => p).ToList();
            SadPlaylist = new Playlist(songList);

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
                case Globals.ActionEvent.ActionPlay: Play(); break;
                case Globals.ActionEvent.ActionStop: Stop(); break;
                case Globals.ActionEvent.ActionPause: Pause(); break;
                case Globals.ActionEvent.ActionPrevious: Previous(); break;
                case Globals.ActionEvent.ActionNext: Next(); break;
                case Globals.ActionEvent.ActionRepeatOn: ToggleRepeat(Globals.State.On); break;
                case Globals.ActionEvent.ActionRepeatOff: ToggleRepeat(Globals.State.Off); break;
            }
            //Set sticky as we are a long running operation
            return StartCommandResult.Sticky;
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
        /// Play song method
        /// <param name="songUrl">The song URL string</param>
        /// </summary>
        private void Play()
        {
            if (networkController.IsConnected)
            {
                try
                {
                    SendCommand(Globals.PlayState.Playing);
                    Play(PlayList.GetCurrentSong().Source);
                    //StartForeground("Playing ",PlayList.GetCurrentSong().Artist, PlayList.GetCurrentSong().Name);
                    networkController.AquireWifiLock();
                }
                catch (Java.Lang.IllegalStateException)
                {
                    Stop();
                }
                catch (Exception ex)
                {
                    Stop();
                }
            }
            else //network is not online
            {
                Stop();
                SendCommand(Globals.Errors.NetworkOffline);
            }
        }

        private async void Play(string songUrl)
        {
            if (State == Globals.PlayState.Paused)
            {
                MediaPlayer.Start();
                State = Globals.PlayState.Playing;
                //StartForeground();
                return;
            }

            if (MediaPlayer.IsPlaying) return;

            try
            {
                await MediaPlayer.SetDataSourceAsync(Application.Context, Android.Net.Uri.Parse(@songUrl));
                var focusResult = audioManager.RequestAudioFocus(this, Stream.Music, AudioFocus.Gain);
                if (focusResult != AudioFocusRequest.Granted)
                {
                    //could not get audio focus
                    throw new Exception("Could not get audio focus");
                }
                MediaPlayer.PrepareAsync();
                //AquireWifiLock();
                //StartForeground();
            }
            catch (Java.Lang.IllegalStateException ex)
            {
                //If the media player is called in an invalid state
                throw ex;
            }
            catch (Exception)
            {
                //unable to start playback log error
                throw new Exception("Unable to start playback log error.");
            }
        }

        /// <summary>
        /// Pause action method
        /// </summary>
        private void Pause()
        {
            SendCommand(Globals.PlayState.Paused);
            if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Pause();
                State = Globals.PlayState.Paused;
            }
            StopForeground(true);
        }


        /// <summary>
        /// Stop action method
        /// </summary>
        private void Stop()
        {
            SendCommand(Globals.PlayState.Stopped);
            if (MediaPlayer.IsPlaying)
                MediaPlayer.Stop();
            MediaPlayer.Reset();
            State = Globals.PlayState.Stopped;
            StopForeground(true);
            networkController.ReleaseWifiLock();
        }

        /// <summary>
        /// Play previous song
        /// </summary>
        private void Previous()
        {
            Stop();
            if (!PlayList.IsAtBeggining)
            {
                PlayList.DecrementPosition();
                Play();
            }
            else if (PlayList.Repeat == Globals.State.On)
            {
                PlayList.SetPositionToEnd();
                Play();
            }
        }

        /// <summary>
        /// Play next song
        /// </summary>
        public void Next()
        {
            Stop();
            if (!PlayList.IsAtEnd)
            {
                PlayList.IncrementPosition();
                Play();
            }
            else if (PlayList.Repeat == Globals.State.On)
            {
                PlayList.ResetPosition();
                Play();
            }
        }

        /// <summary>
        /// Toggle repeat On or Off
        /// </summary>
        /// <param name="state">State can be "On" / "Off"</param>
        private void ToggleRepeat(Globals.State state)
        {
            PlayList.Repeat = state;
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
                throw new Exception();
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
            catch (Exception)
            {
                throw new Exception();
            };
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
            notification.SetLatestEventInfo(Application.Context, title, artist + song, pendingIntent);
#pragma warning restore CS0618 // Type or member is obsolete
            StartForeground(notificationId, notification);
        }
    }
}