using bike.Models;
using Mapsui.UI.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace bike.Controls.SkColorDispersionMap
{
    public interface ISessionDisplayablePoint
    {
        TimeSpan Time { get; }

        Color MapPointColor { get; }

        int? Altitude { get; }

        double? Speed { get; }

        Position Position { get; }

        bool HasMarker { get; }

        string Label { get; }

        int? Distance { get; }
    }

    public class SessionDisplayablePoint : ISessionDisplayablePoint
    {
        private static Position emptyPosition = new Position(0,0);
        public SessionDisplayablePoint(
            TimeSpan timeSpan,
            int? distance,
            int? altitude,
            double? speed,
            Position position,
            bool hasMarker = false,
            string label = null)
        {
            Time = timeSpan;

            Altitude = altitude;
            Speed = speed;
            Position = position;
            Distance = distance;

            MapPointColor = Color.Default;
            HasMarker = hasMarker;
            Label = label;
        }

        public TimeSpan Time { get; }

        public Color MapPointColor { get; private set; }

        public int? Altitude { get; }

        public int? Distance { get; }


        public Position Position { get; }

        public bool HasMarker { get; }

        public string Label { get; }

        public double? Speed { get; }

        public bool HasPosition => Position != emptyPosition;

        public void SetPointColor(Color color)
        {
            MapPointColor = color;
        }
    }
}
