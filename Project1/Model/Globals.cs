using Android.App;
using Android.Widget;
using DisertationProject.Controller;

namespace DisertationProject.Model
{
    /// <summary>
    /// Globals
    /// </summary>
    public static class Globals
    {
        //Strings
        public const string ConnectionString = "Server=tcp:ourserver.database.windows.net,1433;Data Source=ourserver.database.windows.net;Initial Catalog=ourdatabase;Persist Security Info=False;User ID=lanister;Password=tyrion0!;Pooling=False;MultipleActiveResultSets=False;Encrypt=FALSE;Connection Timeout=30;";

        //Ids
        public const int PlayButtonId = Resource.Id.playButton;
        public const int PauseButtonId = Resource.Id.pauseButton;
        public const int StopButtonId = Resource.Id.stopButton;
        public const int PreviousButtonId = Resource.Id.previousButton;
        public const int NextButtonId = Resource.Id.nextButton;
        public const int RepeatButtonId = Resource.Id.repeatButton;
        public const int MainLayoutId = Resource.Layout.Main;
        public const int ErrorTextBox = Resource.Id.textView1;
        public const int SongListView = Resource.Id.songListView;

        //Actions
        public static class ActionEvent
        {
            public const string ActionPlay = "PLAY";
            public const string ActionPause = "PAUSE";
            public const string ActionStop = "ActionSTOP";
            public const string ActionPrevious = "PREVIOUS";
            public const string ActionNext = "NEXT";
            public const string ActionRepeatOn = "REPEAT_ON";
            public const string ActionRepeatOff = "REPEAT_OFF";
            public const string ActionShuffleOn = "SHUFFLE_ON";
            public const string ActionShuffleOff = "SHUFFLE_OFF";
        }

        //Errors
        public static class Errors
        {
            internal const string GeneralException = "GeneralException";
            internal const string IllegalStateException = "IllegalStateException";
            internal const string NetworkOffline = "NetworkOffline";
            internal const string DatabaseError = "DatabaseError";
        }

        //Play state
        public static class PlayState
        {
            internal const string Playing = "Playing";
            internal const string Stopped = "Stopped";
            internal const string Paused = "Paused";

        }

        //Labels
        public const string ProjectLabel = "DisertationProject";

        public static class Songs
        {
            //URL
            public const string SampleSong1 = @"http://www.noiseaddicts.com/samples_1w72b820/4353.mp3";

            //MP3
            public const int SampleSong10 = Resource.Raw.Jessica_Jay_Casablanca;
            public const int SampleSong11 = Resource.Raw.Vaya_Con_Dios_Puerto_Rico;
        }

        //Enumerations
        public enum Emotions { Sad, Happy, Neutral, Angry }
        public enum TextType { Info, Warning, Error }
        public enum State { On, Off }

    }

}