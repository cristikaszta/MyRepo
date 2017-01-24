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
        private ConnectivityManager _connectivityManager;

        /// <summary>
        /// Mobile network information
        /// </summary>
        private NetworkInfo _mobileInfo;

        /// <summary>
        /// Wifi info
        /// </summary>
        private NetworkInfo _wifiInfo;

        /// <summary>
        /// Wifi manager
        /// </summary>
        private WifiManager _wifiManager;

        /// <summary>
        /// Wifi lock used so that music stream oever wifi even when phone is locked
        /// </summary>
        private WifiManager.WifiLock _wifiLock;

        /// <summary>
        /// Contructor
        /// </summary>
        public NetworkController()
        {
            _wifiManager = (WifiManager)MainActivity.Context.GetSystemService(MainActivity.Wifi);
            _connectivityManager = (ConnectivityManager)MainActivity.Context.GetSystemService(MainActivity.Connectivity);
            _wifiLock = _wifiManager.CreateWifiLock(WifiMode.Full, "xamarin_wifi_lock");
            RefreshNetworkInfo();
        }

        /// <summary>
        /// Refresh the network info
        /// To get the status of the connection
        /// </summary>
        private void RefreshNetworkInfo()
        {
            _wifiInfo = _connectivityManager.GetNetworkInfo(ConnectivityType.Wifi);
            _mobileInfo = _connectivityManager.GetNetworkInfo(ConnectivityType.Mobile);
        }

        /// <summary>
        /// Lock the wifi so we can still stream under lock screen
        /// </summary>
        public void AquireWifiLock()
        {
            //if (_wifiLock == null) { _wifiLock = _wifiManager.CreateWifiLock(WifiMode.Full, "xamarin_wifi_lock"); }
            if (!_wifiLock.IsHeld)
                _wifiLock.Acquire();
        }

        /// <summary>
        /// This will release the wifi lock if it is no longer needed
        /// </summary>
        public void ReleaseWifiLock()
        {
            //if (_wifiLock == null)
            //    return;
            if (_wifiLock.IsHeld)
                _wifiLock.Release();
            //_wifiLock = null;
        }

        /// <summary>
        /// Check if connected to internet
        /// </summary>
        /// <returns>True if connected to internet and false otherwise</returns>
        public bool IsOnline()
        {
            RefreshNetworkInfo();
            if (_wifiInfo.IsConnected || _mobileInfo.IsConnected)
                return true;
            else
                return false;
        }

        /// <summary>
        /// On bind
        /// Don't do anything on bind
        /// </summary>
        /// <_parameter name="intent">The intent</_parameter>
        /// <returns></returns>
        //public override IBinder OnBind(Intent intent) { return null; }
    }
}