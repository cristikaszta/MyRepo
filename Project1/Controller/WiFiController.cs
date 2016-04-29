using Android.Net;
using Android.Net.Wifi;

namespace DisertationProject.Controller
{
    public class WiFiController
    {
        public WifiManager wifiManager;
        public WifiManager.WifiLock wifiLock;

        /// <summary>
        /// Lock the wifi so we can still stream under lock screen
        /// </summary>
        public void AquireWifiLock()
        {
            if (wifiLock == null) { wifiLock = wifiManager.CreateWifiLock(WifiMode.Full, "xamarin_wifi_lock"); }
            wifiLock.Acquire();
        }

        /// <summary>
        /// This will release the wifi lock if it is no longer needed
        /// </summary>
        public void ReleaseWifiLock()
        {
            if (wifiLock == null)
                return;

            wifiLock.Release();
            wifiLock = null;
        }
    }
}