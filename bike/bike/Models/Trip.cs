using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Models
{
    public class Trip
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public int DistanceInMeters { get; set; }

        public double MaximumSpeed { get; set; }
        [OneToMany]
        public List<TripDetail> TripDetails { get; set; }
    }
}
