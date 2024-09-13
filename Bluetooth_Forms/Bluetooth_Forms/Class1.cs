using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluetooth_Forms
{
    public class Class1
    {

        public static List<string> ABC (List<string> items)
        {
        BluetoothClient client = new BluetoothClient();
        BluetoothDeviceInfo[] devices = client.DiscoverDevices();
        
            foreach (BluetoothDeviceInfo d in devices)
            {
                d.Refresh();
                items.Add(d.DeviceName);
            }
            return items;
        }



}



}
