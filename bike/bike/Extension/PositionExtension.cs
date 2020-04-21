﻿using bike.Models;
using Syncfusion.SfMaps.XForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Infrastructure.Extension
{
    public static class PositionExtension
    {
        public static LatLong ToLatLong(this Point point)
        {
            if (point.X == 0 && point.Y == 0)
            {
                return LatLong.Empty;
            }

            return new LatLong(point.X, point.Y);
        }

        public static Point ToPoint(this LatLong position)
        {
            if (position == LatLong.Empty)
            {
                return new Point(0, 0);
            }

            return new Point(position.Latitude, position.Longitude);
        }
    }

    public class PositionConverter
    {
        private double _pixelDensity;
        private double _zoomLevel;
        private Point _topLeftPoint;

        public PositionConverter()
        {
        }

        public Size MapSize { get; private set; }

        public Point this[LatLong location]
            => MapToolBox.LatLongToXyAtZoom(location, _zoomLevel, _pixelDensity) - new Size(_topLeftPoint.X, _topLeftPoint.Y);


        public void UpdateCamera(LatLong centerLocation, double zoomLevel, Size mapSize, double pixelDensity)
        {
            _zoomLevel = zoomLevel;
            MapSize = mapSize;
            _pixelDensity = pixelDensity;

            _topLeftPoint = MapToolBox.LatLongToXyAtZoom(centerLocation, zoomLevel, _pixelDensity) -
                new Size(mapSize.Width / 2, mapSize.Height / 2);
        }

        public override string ToString()
            => $"topLeft: {_topLeftPoint}, zoom: {_zoomLevel}, density: {_pixelDensity}";
    }

    internal static class MapToolBox
    {
        /// <summary>
        /// The radius of the earth (in meters) - should never change!
        /// </summary>
        private const double EarthRadius = 6378137d;

        /// <summary>
        /// calculated circumference of the earth
        /// </summary>
        private const double EarthCircumference = EarthRadius * 2d * Math.PI;

        private const double EarthHalfCircumference = EarthCircumference / 2d;
        private const int TileSize = 256;

        public static Point LatLongToXyAtZoom(LatLong latLong, double zoom, double pixelDensity)
        {
            Debug.Assert(zoom >= 0, "Expecting positive zoom factor");

            int pixelsPerTile = (int)(TileSize * pixelDensity);

            // double arc = VirtualEarthToolBox.earthCircumference / ((1 << zoom) * pixelsPerTile);
            var arc = EarthCircumference / (Math.Pow(2, zoom) * pixelsPerTile);
            var sinLat = Math.Sin(latLong.Latitude * Math.PI / 180d);
            var metersY = EarthRadius / 2 * Math.Log((1 + sinLat) / (1 - sinLat));
            var metersX = EarthRadius * latLong.Longitude * Math.PI / 180d;

            return new Point(
                (int)((EarthHalfCircumference + metersX) / arc),
                (int)((EarthHalfCircumference - metersY) / arc));
        }
    }
}