using Android.App;
using Android.Net;
using Android.Net.Wifi;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Network controller class
    /// </summary>
    public class NetworkController
    {
        /// <summary>
        /// Conectivity manager
        /// </summary>
        private ConnectivityManager connectivityManager;

        /// <summary>
        /// Wifi manager
        /// </summary>
        private WifiManager wifiManager;

        /// <summary>
        /// Wifi lock used so that music stream oever wifi even when phone is locked
        /// </summary>
        private WifiManager.WifiLock wifiLock;


        /// <summary>
        /// Check is connected to the network
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (connectivityManager.GetNetworkInfo(ConnectivityType.Wifi).IsConnected ||
                    connectivityManager.GetNetworkInfo(ConnectivityType.Mobile).IsConnected)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkController()
        {
            wifiManager = (WifiManager)Application.Context.GetSystemService(Android.Content.Context.WifiService);
            connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Android.Content.Context.ConnectivityService);
            wifiLock = wifiManager.CreateWifiLock(WifiMode.Full, "Wifi lock");
        }

        /// <summary>
        /// Lock the wifi so we can still stream under lock screen
        /// </summary>
        public void AquireWifiLock()
        {
            if (!wifiLock.IsHeld)
                wifiLock.Acquire();
        }

        /// <summary>
        /// This will release the wifi lock if it is no longer needed
        /// </summary>
        public void ReleaseWifiLock()
        {
            if (wifiLock.IsHeld)
                wifiLock.Release();
        }
    }
}