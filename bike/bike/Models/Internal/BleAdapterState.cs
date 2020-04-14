using Shiny;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Models.Logging
{
    public class BleAdapterState
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public AccessState State { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
