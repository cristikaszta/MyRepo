using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DisertationProject.Data
{
    /// <summary>
    /// Ids 
    /// </summary>
    public class Globals
    {
        //Integers
        public const int PlayButtonId = Resource.Id.playButton;
        public const int PauseButtonId = Resource.Id.pauseButton;
        public const int StopButtonId = Resource.Id.stopButton;
        public const int MainLayoutId = Resource.Layout.Main;

        //Strings
        public const string ActionPlay = "com.xamarin.action.PLAY";
        public const string ActionPause = "com.xamarin.action.PAUSE";
        public const string ActionStop = "com.xamarin.action.STOP";
        public const string SampleSong = @"http://www.montemagno.com/sample.mp3";
        public const string ProjectLabel = "DisertationProject";

    }


}