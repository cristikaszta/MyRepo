using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Project1.Services.StreamingService;

namespace Project1
{
    [Activity(Label = "Project1", MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var play = FindViewById<Button>(Resource.Id.playButton);
            var pause = FindViewById<Button>(Resource.Id.pauseButton);
            var stop = FindViewById<Button>(Resource.Id.stopButton);

            play.Click += (sender, args) => SendAudioCommand(StreamingBackgroundService.ActionPlay);
            pause.Click += (sender, args) => SendAudioCommand(StreamingBackgroundService.ActionPause);
            stop.Click += (sender, args) => SendAudioCommand(StreamingBackgroundService.ActionStop);

        }

        private void SendAudioCommand(string action)
        {
            var intent = new Intent(action);
            StartService(intent);
        }
    }
}

