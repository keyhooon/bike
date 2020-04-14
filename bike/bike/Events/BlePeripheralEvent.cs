using Shiny.BluetoothLE.Central;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Events
{
    public class BlePeripheralEvent : Prism.Events.PubSubEvent<IPeripheral>
    {
    }
}
