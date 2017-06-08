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
        /// Mobile network information
        /// </summary>
        private NetworkInfo mobileInfo;

        /// <summary>
        /// Wifi info
        /// </summary>
        private NetworkInfo wifiInfo;

        /// <summary>
        /// Wifi manager
        /// </summary>
        private WifiManager wifiManager;

        /// <summary>
        /// Wifi lock used so that music stream oever wifi even when phone is locked
        /// </summary>
        private WifiManager.WifiLock wifiLock;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkController()
        {
            wifiManager = (WifiManager)MainActivity.Context.GetSystemService(MainActivity.Wifi);
            connectivityManager = (ConnectivityManager)MainActivity.Context.GetSystemService(MainActivity.Connectivity);
            wifiLock = wifiManager.CreateWifiLock(WifiMode.Full, "Wifi lock");
            RefreshNetworkInfo();
        }

        /// <summary>
        /// Refresh the network info to get the status of the connection
        /// </summary>
        private void RefreshNetworkInfo()
        {
            wifiInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi);
            mobileInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Mobile);
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

        /// <summary>
        /// Check if connected to internet
        /// </summary>
        /// <returns>True if connected to internet and false otherwise</returns>
        public bool IsOnline()
        {
            RefreshNetworkInfo();
            if (wifiInfo.IsConnected || mobileInfo.IsConnected)
                return true;
            else
                return false;
        }

    }
}