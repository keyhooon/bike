using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Events
{
    public class GpsDataReceivedEvent : Prism.Events.PubSubEvent<IGpsReading>
    {
    }
}
