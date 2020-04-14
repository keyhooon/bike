using bike.Events;
using bike.Models.Logging;
using Prism.Events;
using Shiny;
using Shiny.BluetoothLE.Central;
using Shiny.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bike.Shiny.Delegate
{
    public class BleDelegate : IBleCentralDelegate
    {
        readonly CoreDelegateServices services;
        private readonly SqliteConnection connection;
        private readonly IEventAggregator eventAggregator;

        public BleDelegate(CoreDelegateServices services, SqliteConnection connection, IEventAggregator eventAggregator)
        {
            this.services = services;
            this.connection = connection;
            this.eventAggregator = eventAggregator;
        }

        public async Task OnAdapterStateChanged(AccessState state)
        {
            await connection.InsertAsync(new BleAdapterState() 
            { 
                State = state, 
                Timestamp = DateTime.Now 
            });
            eventAggregator.GetEvent<BleAdapterStateChangedEvent>().Publish(state);
        }


        public async Task OnConnected(IPeripheral peripheral)
        {
            await connection.InsertAsync(new BleConnectedPeripheral
            {
                Name = peripheral.Name,
                Uuid = peripheral.Uuid,
                MtuSize = peripheral.MtuSize,
                Status = peripheral.Status,
                Timestamp = DateTime.Now
            });
            eventAggregator.GetEvent<BlePeripheralEvent>().Publish(peripheral);
        }
    }
}
