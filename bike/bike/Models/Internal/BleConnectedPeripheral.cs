using Shiny.BluetoothLE;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Models.Logging
{
    public class BleConnectedPeripheral
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid Uuid { get; set; }
        public ConnectionState Status { get; set; }
        public int MtuSize { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
