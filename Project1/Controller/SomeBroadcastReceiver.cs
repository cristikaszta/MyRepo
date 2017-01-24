using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using DisertationProject.Model;

namespace DisertationProject.Controller
{
    [IntentFilter(new[] { Globals.TheAction })]
    public class SomeBroadcastReceiver : BroadcastReceiver
    {
        public event EventHandler SomeDataReceived;

        public override void OnReceive(Context context, Intent intent)
        {
            switch (intent.Action)
            {
                case Globals.TheAction:
                    //context.
                    //OnSomeDataReceived(new SomeDataEventArgs
                    //{
                    //    Data = intent.GetIntExtra(Globals.DataToPassToActivity, .S 0)
                    //});
                    break;
            }
        }

        protected virtual void OnSomeDataReceived(EventArgs e)
        {
            if (SomeDataReceived != null)
            {
                SomeDataReceived(this, e);
            }
        }
    }
}