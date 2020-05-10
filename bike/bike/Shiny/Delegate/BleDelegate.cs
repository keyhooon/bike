using bike.Events;
using bike.Models.Logging;
using Prism.Events;
using Shiny;
using Shiny.BluetoothLE;
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
        private readonly ILogger logger;

        public BleDelegate(CoreDelegateServices services, SqliteConnection connection, IEventAggregator eventAggregator, ILogger logger)
        {
            this.services = services;
            this.connection = connection;
            this.eventAggregator = eventAggregator;
            this.logger = logger;
        }

        public async Task OnAdapterStateChanged(AccessState state)
        {
            logger.Write("BleAdapterStateChanged", "", new [] { ("AccessState", Enum.GetName(typeof(AccessState), state)) });
            //await connection.InsertAsync(new BleAdapterState() 
            //{ 
            //    State = state, 
            //    Timestamp = DateTime.Now 
            //});
            eventAggregator.GetEvent<BleAdapterStateChangedEvent>().Publish(state);
        }


        public async Task OnConnected(IPeripheral peripheral)
        {
            logger.Write("BleConnectedChanged", "", new [] {
                ("Name", peripheral.Name),
                ("Uuid", peripheral.Uuid.ToString()),
                ("MtuSize", peripheral.MtuSize.ToString()),
                ("Status", Enum.GetName(typeof(ConnectionState), peripheral.Status))});

            //await connection.InsertAsync(new BleConnectedPeripheral
            //{
            //    Name = peripheral.Name,
            //    Uuid = peripheral.Uuid,
            //    MtuSize = peripheral.MtuSize,
            //    Status = peripheral.Status,
            //    Timestamp = DateTime.Now
            //});
            eventAggregator.GetEvent<BlePeripheralEvent>().Publish(peripheral);
        }
    }
}
