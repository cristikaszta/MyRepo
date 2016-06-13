using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Wifi controller class
    /// </summary>
    public class WiFiController : Service
    {
        /// <summary>
        /// Wifi manager
        /// </summary>
        public WifiManager wifiManager;

        /// <summary>
        /// Wifi lock used so that music stream oever wifi even when phone is locked
        /// </summary>
        public WifiManager.WifiLock wifiLock;

        /// <summary>
        /// Contructor
        /// </summary>
        public WiFiController()
        {
            wifiManager = (WifiManager)MainController.Context.GetSystemService(MainController.Wifi);
            wifiLock = wifiManager.CreateWifiLock(WifiMode.Full, "xamarin_wifi_lock");
        }

        /// <summary>
        /// Lock the wifi so we can still stream under lock screen
        /// </summary>
        public void AquireWifiLock()
        {
            //if (wifiLock == null) { wifiLock = wifiManager.CreateWifiLock(WifiMode.Full, "xamarin_wifi_lock"); }
            if (!wifiLock.IsHeld)
                wifiLock.Acquire();
        }

        /// <summary>
        /// This will release the wifi lock if it is no longer needed
        /// </summary>
        public void ReleaseWifiLock()
        {
            //if (wifiLock == null)
            //    return;
            if (wifiLock.IsHeld)
                wifiLock.Release();
            //wifiLock = null;
        }

        /// <summary>
        /// On bind
        /// Don't do anything on bind
        /// </summary>
        /// <_parameter name="intent">The intent</_parameter>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent) { return null; }
    }
}