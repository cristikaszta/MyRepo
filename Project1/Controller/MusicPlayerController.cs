using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using System;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Music player controller
    /// </summary>
    public class MusicPlayerController : Service, AudioManager.IOnAudioFocusChangeListener
    {
        /// <summary>
        /// The media player
        /// </summary>
        public MediaPlayer MediaPlayer;

        /// <summary>
        /// The audio manager
        /// </summary>
        private AudioManager _audioManager;

        /// <summary>
        /// Is paused flag
        /// </summary>
        private bool _isPaused;

        /// <summary>
        /// Contructor
        /// </summary>
        public MusicPlayerController()
        {
            MediaPlayer = new MediaPlayer();
            _audioManager = (AudioManager)MainController.Context.GetSystemService(MainController.Audio);
            InitializeMediaPlayer();
        }

        /// <summary>
        /// On bind
        /// Don't do anything on bind
        /// </summary>
        /// <param name="intent">The intent</param>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent) { return null; }

        /// <summary>
        /// Initialize player
        /// </summary>
        public void InitializeMediaPlayer()
        {
            MediaPlayer = new MediaPlayer();

            //Tell our player to stream music
            MediaPlayer.SetAudioStreamType(Stream.Music);

            //Wake mode will be partial to keep the CPU still running under lock screen
            MediaPlayer.SetWakeMode(MainController.Context, WakeLockFlags.Partial);

            //When we have prepared the song start playback
            MediaPlayer.Prepared += (sender, args) => MediaPlayer.Start();

            //When we have reached the end of the song stop ourselves, however you could signal next track here.
            //MediaPlayer.Completion += (sender, args) => Stop();
            //MediaPlayer.Error += (sender, args) =>
            //{
            //    //playback error
            //    Console.WriteLine("Error in playback resetting: " + args.What);
            //    Stop();//this will clean up and reset properly.
            //};
        }

        /// <summary>
        /// Play song method
        /// </summary>
        public async void Play(string songUrl) 
        {
            if (_isPaused)// && mediaPlayer != null)
            {
                _isPaused = false;
                //We are simply paused so just start again
                MediaPlayer.Start();
                //StartForeground();
                return;
            }

            //if (mediaPlayer == null) { InitializePlayer(); }
            if (MediaPlayer.IsPlaying) return;

            try
            {
                await MediaPlayer.SetDataSourceAsync(MainController.Context, Android.Net.Uri.Parse(songUrl));
                var focusResult = _audioManager.RequestAudioFocus(this, Stream.Music, AudioFocus.Gain);
                if (focusResult != AudioFocusRequest.Granted)
                {
                    //could not get audio focus
                    //Console.WriteLine("Could not get audio focus");
                    throw new Exception();
                }
                MediaPlayer.PrepareAsync();
                //AquireWifiLock();
                //StartForeground();
            }
            catch(Java.Lang.IllegalStateException)
            {
                //If the media player is called in an invalid state
                throw new Java.Lang.IllegalStateException();
            }
            catch (Exception)
            {
                //General exception
                throw new Exception();
            }
        }

        /// <summary>
        /// Pause song method
        /// </summary>
        public void Pause()
        {
            //if (MediaPlayer == null)
            //    return;
            if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Pause();
                _isPaused = true;
            }
        }

        /// <summary>
        /// Stop song method
        /// </summary>
        public void Stop()
        {
            //if (MediaPlayer == null)
            //    return;
            if (MediaPlayer.IsPlaying)
                MediaPlayer.Stop();
            MediaPlayer.Reset();
            _isPaused = false;
        }

        /// <summary>
        /// Properly cleanup of your player by releasing resources
        /// </summary>
        //public override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    if (mediaPlayer != null)
        //    {
        //        mediaPlayer.Release();
        //        mediaPlayer = null;
        //    }
        //}

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
                    //if (MediaPlayer == null) InitializePlayer();

                    if (!MediaPlayer.IsPlaying)
                    {
                        MediaPlayer.Start();
                        _isPaused = false;
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
        /// Get the duration of the current song
        /// </summary>
        /// <returns>The duration of the current playing song</returns>
        public int GetSongDuration()
        {
            var duration = 0;
            try
            {
                duration = MediaPlayer.Duration;
            }
            catch(Exception)
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

    }
}