﻿using Mapsui;
using Mapsui.Projection;
using Mapsui.UI;
using Mapsui.Utilities;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;
using Mapsui.Widgets;
using Mapsui.UI.Forms;
using Mapsui.Rendering.Skia;
using Prism.Events;
using bike.Events;
using System;
using Mapsui.Geometries;
using Shiny.Locations;

namespace bike.Views
{
    public partial class MapPage 
    {
        private readonly IEventAggregator eventAggregator;

        public MapPage(IGpsManager manager, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            mapView.Map = CreateMap();
            manager.RequestAccessAndStart(new GpsRequest() { Interval = TimeSpan.FromSeconds(5), UseBackground = true });

            mapView.Navigator = new Navigator(mapView.Map, (IViewport)mapView.Viewport);
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<GpsDataReceivedEvent>().Subscribe(e =>
           {
               Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
               {
                   var coords = new Position(e.Position.Latitude, e.Position.Longitude);
                   info.Text = $"{coords.ToString()} - D:{(int)e.Heading} S:{Math.Round(e.Speed, 2)}";

                   mapView.MyLocationLayer.UpdateMyLocation(new Position(e.Position.Latitude, e.Position.Longitude));
                   mapView.MyLocationLayer.UpdateMyDirection(e.Heading, mapView.Viewport.Rotation);
                   mapView.MyLocationLayer.UpdateMySpeed(e.Speed);
               });
           });
        }
        public static Map CreateMap()
        {
            var map = new Map
            {
                CRS = "EPSG:3857",
                Transformation = new MinimalTransformation()
            };
            map.Limiter.PanLimits = new BoundingBox(new Point(45.26, 24.68), new Point(60.56, 39.26));
            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            map.Widgets.Add(new ScaleBarWidget(map) { TextAlignment = Alignment.Center, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top });
            map.Widgets.Add(new ZoomInOutWidget { MarginX = 20, MarginY = 40 });
            return map;
        }
        protected override void OnAppearing()
        {
            mapView.Refresh();
        }
        //private void mapView_MapClicked(object sender, MapClickedEventArgs args)
        //{
        //    var mapView = sender as MapView;
        //    var e = args as MapClickedEventArgs;
        //    var navigator = (AnimatedNavigator)mapView.Navigator;
        //    navigator.FlyTo(e.Point.ToMapsui(), mapView.Viewport.Resolution * 2);
        //}

        //private void mapView_PinClicked(object sender, PinClickedEventArgs e)
        //{
        //    if (e.Pin != null)
        //    {
        //        if (e.NumOfTaps == 2)
        //        {
        //            // Hide Pin when double click
        //            //DisplayAlert($"Pin {e.Pin.Label}", $"Is at position {e.Pin.Position}", "Ok");
        //            e.Pin.IsVisible = false;
        //        }
        //        if (e.NumOfTaps == 1)
        //            if (e.Pin.Callout.IsVisible)
        //                e.Pin.HideCallout();
        //            else
        //                e.Pin.ShowCallout();
        //    }

        //    e.Handled = true;
        //}

        //private void mapView_Info(object sender, Mapsui.UI.MapInfoEventArgs e)
        //{
        //    if (e?.MapInfo?.Feature != null)
        //    {
        //        foreach (var style in e.MapInfo.Feature.Styles)
        //        {
        //            if (style is CalloutStyle)
        //            {
        //                style.Enabled = !style.Enabled;
        //                e.Handled = true;
        //            }
        //        }

        //        mapView.Refresh();
        //    }
        //}

    }
}
