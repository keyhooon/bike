using Mapsui.Geometries;
using Mapsui.UI.Forms;
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

        public TimeSpan TimeStamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude{ get; set; }

        public int DistanceInMeters { get; set; }

        public int AltitudeInMeters { get; set; }

        public double? Speed { get; set; }
    }
}
