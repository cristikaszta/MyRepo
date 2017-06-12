using Android.App;
using Android.Content;
using Android.OS;
using Android.Webkit;
using DisertationProject.Model;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = Globals.ProjectLabel, MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class MainActivity : Activity
    {
        /// <summary>
        /// The instance of the main activity
        /// </summary>
        private static MainActivity instance { get; set; }

        /// <summary>
        /// Get instance of main activity
        /// </summary>
        /// <returns>Instance of main activity</returns>
        public static MainActivity getInstace()
        {
            return instance;
        }

        WebView webView;
        WebSettings webSettings;

        /// <summary>
        /// On create
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Globals.MainLayoutId);

            //Set instance
            instance = this;

            //Start music service controller
            var firstIntent = new Intent(Globals.ActionEvent.ActionStop);
            firstIntent.SetPackage("DisertationProject.Controller");
            Application.Context.StartService(firstIntent);

            //Start UI interface controller
            var secondIntent = new Intent(Globals.PlayState.Stopped);
            secondIntent.SetPackage("DisertationProject.Controller");
            Application.Context.StartService(secondIntent);

            //webView = FindViewById<WebView>(Resource.Id.webView1);
            //webSettings = webView.Settings;
            //webSettings.JavaScriptEnabled = true;
            //webView.SetWebChromeClient(new WebChromeClient());
            //webView.LoadUrl(@"https://www.youtube.com/embed/gTw2YvutJRA?ecver=2");
        }

        /// <summary>
        /// Override OnDestroy method
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        /// <summary>
        /// Override OnPause method
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

    }
}