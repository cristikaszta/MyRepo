using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using DisertationProject.Model;
using Java.IO;
using System;
using System.Collections.Generic;

using Android.Content.PM;
using Android.Provider;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using CameraAppDemo;

namespace DisertationProject.Controller
{
   
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = Globals.PROJECT_LABEL, MainLauncher = true, Icon = "@drawable/ic_launcher")]

    public class MainActivity : Activity
    {
        private ImageView _imageView;

        /// <summary>
        /// Textview object which containes the text
        /// </summary>
        private TextView _textView;

        /// <summary>
        /// Song list adapter
        /// </summary>
        private SongListAdapter _songListAdapter;

        /// <summary>
        /// The list view
        /// </summary>
        private ListView _listView;

        /// <summary>
        /// Playlist
        /// </summary>
        public PlayList _playList { get; set; }

        private SongCompletionReceiver _songCompletionReceiver;
       

        /// <summary>
        /// On create
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Globals.MainLayoutId);
            GetSongList();
            SetupButtons();
            SetupTextContainers();

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                Button button = FindViewById<Button>(Resource.Id.cameraButton);
                _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
                button.Click += TakeAPicture;
            }

            _songCompletionReceiver = new SongCompletionReceiver();
            _songCompletionReceiver.SongCompletionEventHandler += (sender, args) =>
            {
                if (!_playList.IsAtEnd)
                {
                    _playList.IncrementPosition();
                }
                else if (_playList.Repeat == ToggleState.On)
                {
                    _playList.ResetPosition();
                }
                else
                {
                    return;
                }
                _playList.IncrementPosition();
                //var source = _playList.GetCurrentSong().Source;
                var  source = Globals.Songs.SampleSong1;
                SendCommand(ActionEvent.ActionPlay, source);
            };

            RegisterReceiver(_songCompletionReceiver, new IntentFilter("GetNext"));
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory 
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = _imageView.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null)
            {
                _imageView.SetImageBitmap(App.bitmap);
                App.bitmap = null;
            }

            // Dispose of the Java side bitmap.
            GC.Collect();
        }

        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
                Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));

            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));

            StartActivityForResult(intent, 0);
        }

        private void GetSongList()
        {
            var response = DataController.Instance.GetSongs();
            if (response.Status == GenericStatus.Success)
            {
                _playList = new PlayList
                {
                    SongList = response.Result
                };
            };
        }

        private void SetupButtons()
        {
            var playButton = FindViewById<Button>(Resource.Id.playButton);
            playButton.Click += (sender, args) =>
            {
                var name = _playList.GetCurrentSong().Name;
                var source = _playList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionPlay, source, name);
            };

            var pauseButton = FindViewById<Button>(Resource.Id.pauseButton);
            pauseButton.Click += (sender, args) => SendCommand(ActionEvent.ActionPause);

            var stopButton = FindViewById<Button>(Resource.Id.stopButton);
            stopButton.Click += (sender, args) => SendCommand(ActionEvent.ActionStop);

            var previousButton = FindViewById<Button>(Resource.Id.previousButton);
            previousButton.Click += (sender, args) =>
            {
                if (!_playList.IsAtBeggining)
                {
                    _playList.DecrementPosition();
                }
                else if (_playList.Repeat == ToggleState.On)
                {
                    _playList.SetPositionToEnd();
                }
                else
                {
                    return;
                }
                var name = _playList.GetCurrentSong().Name;
                var source = _playList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionStop);
                SendCommand(ActionEvent.ActionPlay, source, name);
            };

            var nextButton = FindViewById<Button>(Resource.Id.nextButton);
            nextButton.Click += (sender, args) =>
            {
                if (!_playList.IsAtEnd)
                {
                    _playList.IncrementPosition();
                }
                else if (_playList.Repeat == ToggleState.On)
                {
                    _playList.ResetPosition();
                }
                else
                {
                    return;
                }
                var name = _playList.GetCurrentSong().Name;
                var source = _playList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionStop);
                SendCommand(ActionEvent.ActionPlay, source, name);
            };

            var repeatButton = FindViewById<ToggleButton>(Resource.Id.repeatButton);
            repeatButton.Click += (sender, args) =>
            {
                //if (Repeat.Checked)
                //    SendCommand(ActionEvent.ActionRepeatOn);
                //else SendCommand(ActionEvent.ActionRepeatOff);
                if (repeatButton.Checked)
                    _playList.Repeat = ToggleState.On;
                else
                    _playList.Repeat = ToggleState.Off;
            };

            var shuffleButton = FindViewById<ToggleButton>(Resource.Id.shuffleButton);
            shuffleButton.Click += (sender, args) =>
            {
                //if (Repeat.Checked) SendCommand(ActionEvent.ActionShuffleOn);
                //else SendCommand(ActionEvent.ActionShuffleOff);
                if (repeatButton.Checked)
                    _playList.Shuffle = ToggleState.On;
                else
                    _playList.Shuffle = ToggleState.Off;
            };
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void SetupTextContainers()
        {
            _textView = FindViewById<TextView>(Resource.Id.textView1);
            _listView = FindViewById<ListView>(Resource.Id.songListView);
            _songListAdapter = new SongListAdapter(this, _playList.SongList);
            _listView.Adapter = _songListAdapter;
            _listView.ItemClick += (sender, args) =>
            {
                var currentPosition = args.Position;
                _playList.Position = currentPosition;
                var name = _playList.GetCurrentSong().Name;
                var source = _playList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionPlay, source, name);
            };
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
    public class SongCompletionReceiver : BroadcastReceiver
    {
        public event EventHandler SongCompletionEventHandler;

        public override void OnReceive(Context context, Intent intent)
        {
            var e = new EventArgs();
            SongCompletionEventHandler?.Invoke(this, e);
            //OnSomeDataReceived(new EventArgs());
        }

        //protected virtual void OnSomeDataReceived(EventArgs e)
        //{
        //    SongCompletionEventHandler?.Invoke(this, e);
        //}
    }

    public static class App
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }
}
