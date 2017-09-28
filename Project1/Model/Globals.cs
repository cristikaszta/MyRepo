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


        //Labels
        public const string PROJECT_LABEL = "DisertationProject";

        public static class Songs
        {
            //URL
            public const string SampleSong1 = @"http://www.noiseaddicts.com/samples_1w72b820/4353.mp3";
            public const string SampleSong2 = @"http://www.noiseaddicts.com/samples_1w72b820/1450.mp3";
            public const string SampleSong3 = @"http://www.noiseaddicts.com/samples_1w72b820/4356.mp3";
            public const string SampleSong4 = @"http://www.noiseaddicts.com/samples_1w72b820/4249.mp3";

            //MP3
            public const int SampleSong10 = Resource.Raw.Jessica_Jay_Casablanca;
            public const int SampleSong11 = Resource.Raw.Vaya_Con_Dios_Puerto_Rico;
        }
    }

}