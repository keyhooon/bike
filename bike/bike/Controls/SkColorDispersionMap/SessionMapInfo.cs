using bike.Helper;
using bike.Models;
using Mapsui.UI.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Infrastructure.Extension;
using Mapsui.UI.Objects;
using Mapsui.Providers;

namespace bike.Controls.SkColorDispersionMap
{
    public class SessionLineString : BindableObject, IFeatureProvider
    {
        public static readonly BindableProperty SessionProperty = BindableProperty.Create(nameof(Session), typeof(Session), typeof(SessionLineString), null);

        public SessionLineString(Session session)
        {

        }

        public Session Session
        {
            get { return (Session)GetValue(SessionProperty); }
            set { SetValue(SessionProperty, value); }
        }


        private object sync = new object();



        private void CreateFeature()
        {
            lock (sync)
            {
                if (Feature == null)
                {
                    // Create a new one
                    Feature = new Feature
                    {
                        Geometry = new MultiWeigthLineString(){LineStrings = new multi,
                    };
                    Feature.Styles.Clear();
                    Feature.Styles.Add(new VectorStyle
                    {
                        Line = new Pen { Width = StrokeWidth, Color = StrokeColor.ToMapsui() },
                        Fill = new Brush { Color = FillColor.ToMapsui() }
                    });
                }
            }
        }

        private Feature feature;

        /// <summary>
        /// Mapsui Feature belonging to this drawable
        /// </summary>
        public Feature Feature
        {
            get
            {
                return feature;
            }
            set
            {
                if (feature == null || !feature.Equals(value))
                    feature = value;
            }
        }

        public static SessionMapInfo Create(
            IReadOnlyList<SessionDetail> datas,
            Func<ISessionDisplayablePoint, Color?> colorBaseValueSelector,
            int markerInterval = int.MaxValue,
            int displayDistanceInterval = int.MaxValue)
        {
            if (datas == null || datas.Count < 2)
            {
                throw new ArgumentException();
            }

            var sessionPoints = new SessionDisplayablePoint[datas.Count];

            double topLatitude = LatLong.Min.Latitude;
            double bottomLatitude = LatLong.Max.Latitude;
            double leftLongitude = LatLong.Max.Longitude;
            double rightLongitude = LatLong.Min.Longitude;

            int nextMarkerDistance = markerInterval;
            int nextDisplayDistance = displayDistanceInterval;

            SessionDisplayablePoint previousPoint = null;
            DateTime startTime = datas[0].TimeStamp;
            for (int index = 0; index < datas.Count; index++)
            {
                SessionDetail data = datas[index];

                if (data.LatLong != LatLong.Empty)
                {
                    topLatitude = Math.Max(data.LatLong.Latitude, topLatitude);
                    bottomLatitude = Math.Min(data.LatLong.Latitude, bottomLatitude);
                    leftLongitude = Math.Min(data.LatLong.Longitude, leftLongitude);
                    rightLongitude = Math.Max(data.LatLong.Longitude, rightLongitude);
                }

                bool hasMarker = false;
                if (data.DistanceInMeters > nextMarkerDistance)
                {
                    hasMarker = true;
                    nextMarkerDistance += markerInterval;
                }

                string displayDistance = null;
                if (data.DistanceInMeters > nextDisplayDistance)
                {
                    displayDistance = nextDisplayDistance.ToString();
                    nextDisplayDistance += displayDistanceInterval;
                }

                TimeSpan elapsedTime = data.TimeStamp - startTime;

                double? speed = data.Speed;
                if (speed == null
                    && previousPoint != null
                    && previousPoint.HasPosition
                    && previousPoint.Distance.HasValue
                    && data.LatLong != LatLong.Empty
                    && data.DistanceInMeters > 0
                    && elapsedTime.TotalSeconds > 0)
                {
                    double kilometersTraveled =
                        PositionHelper.HaversineDistance(previousPoint.Position, data.LatLong.ToPosition());
                    double hoursElapsed = (elapsedTime - previousPoint.Time).TotalHours;
                    speed = kilometersTraveled / hoursElapsed;
                }

                var currentPoint = sessionPoints[index] = new SessionDisplayablePoint(
                    elapsedTime,
                    data.DistanceInMeters,
                    data.AltitudeInMeters,
                    speed, 
                    data.LatLong.ToPosition(),
                    hasMarker,
                    displayDistance);

                Color mapPointColor =
                    colorBaseValueSelector(currentPoint) ?? previousPoint?.MapPointColor ?? Color.Gray;

                currentPoint.SetPointColor(mapPointColor);
                previousPoint = currentPoint;
            }

            return new SessionMapInfo(
                sessionPoints,
                new Position(bottomLatitude, leftLongitude),
                new Position(topLatitude, rightLongitude),
                previousPoint != null ? (int)previousPoint.Time.TotalSeconds : 0);
        }

        public IReadOnlyList<SessionDisplayablePoint> SessionPoints { get; }

        public MapSpan Region { get; }

        public Position BottomLeft { get; }

        public Position TopRight { get; }

        public int TotalDurationInSeconds { get; }
    }
}
