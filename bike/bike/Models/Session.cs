using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Models
{
    public class Session
    {
        [PrimaryKey]
        [AutoIncrement]
        public string Id { get; set; }

        public DateTime LastPointTime { get; set; }

        public TimeSpan Duration { get; set; }

        public int DistanceInMeters { get; set; }

        public double MaximumSpeed { get; set; }

    }
}
