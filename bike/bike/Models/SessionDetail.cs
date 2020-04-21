using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Models
{
    public class SessionDetail
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public LatLong Position { get; set; }

        public int DistanceInMeters { get; set; }

        public int AltitudeInMeters { get; set; }

        public double? Speed { get; set; }
    }
}
