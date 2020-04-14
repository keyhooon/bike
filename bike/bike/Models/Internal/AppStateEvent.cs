using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Models.Logging
{
    public class AppStateEvent
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Event { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
