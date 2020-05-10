using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using bike.Events;
using bike.Models.Logging;
using Prism.Events;
using Shiny.Locations;
using Shiny.Logging;

namespace bike.Shiny.Delegate
{
    public class GpsDelegate : IGpsDelegate
    {
        private readonly SqliteConnection connection;
        private readonly IEventAggregator eventAggregator;
        private readonly ILogger logger;

        public GpsDelegate(SqliteConnection connection, IEventAggregator eventAggregator, ILogger logger)
        {
            this.connection = connection;
            this.eventAggregator = eventAggregator;
            this.logger = logger;
        }
        public async Task OnReading(IGpsReading reading)
        {
            //await connection.InsertAsync(new GpsData()
            //{
            //    Latitude = reading.Position.Latitude,
            //    Longitude = reading.Position.Longitude,
            //    Altitude = reading.Altitude,
            //    PositionAccuracy = reading.PositionAccuracy,
            //    Heading = reading.Heading,
            //    HeadingAccuracy = reading.HeadingAccuracy,
            //    Speed = reading.Speed,
            //    Date = reading.Timestamp.ToLocalTime()
            //});
            eventAggregator.GetEvent<GpsDataReceivedEvent>().Publish(reading);
        }
    }
}
