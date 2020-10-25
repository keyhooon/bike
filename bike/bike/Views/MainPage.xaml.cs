using Infrastructure;
using Mapsui;
using Mapsui.Geometries;
using Mapsui.Projection;
using Mapsui.Utilities;
using Mapsui.Widgets;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;
using Prism.Events;
using bike.Events;
using Mapsui.UI.Forms;
using System;
using Shiny.Locations;
using Xamarin.Forms.Xaml;

namespace bike.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage 
    {
        public MainPage(IGpsManager manager, IEventAggregator eventAggregator)
        {
            InitializeComponent();

            var map = new Map
            {
                CRS = "EPSG:3857",
                Transformation = new MinimalTransformation()
            };

           // map.Limiter.PanLimits = new BoundingBox(new Mapsui.Geometries.Point(45.26, 24.68), new Mapsui.Geometries.Point(60.56, 39.26));
            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            
            map.Widgets.Add(new ScaleBarWidget(map) { TextAlignment = Alignment.Center, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top });
            map.Widgets.Add(new ZoomInOutWidget { MarginX = 20, MarginY = 40 });
            mapView.Map = map;
            
            manager.RequestAccessAndStart(new GpsRequest());
            manager.StartListener(new GpsRequest());

            mapView.Navigator = new Navigator(mapView.Map, (IViewport)mapView.Viewport);
            
            eventAggregator.GetEvent<GpsDataReceivedEvent>().Subscribe(e =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    var coords = new Position(e.Position.Latitude, e.Position.Longitude);
                    info.Text = $"{coords} - D:{(int)e.Heading} S:{Math.Round(e.Speed, 2)}";

                    mapView.MyLocationLayer.UpdateMyLocation(new Position(e.Position.Latitude, e.Position.Longitude));
                    mapView.MyLocationLayer.UpdateMyDirection(e.Heading, mapView.Viewport.Rotation);
                    mapView.MyLocationLayer.UpdateMySpeed(e.Speed);
                });
            });
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            //this.Detail.Navigation.PushAsync(new SettingPage());
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            ((Element)sender).GetParent<Grid>().GetChildren<Picker>().FirstOrDefault()?.Focus();

        }
        private void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ((IVisualElementController)sender).InvalidateMeasure(InvalidationTrigger.MeasureChanged);
        }

    }
}