using Shiny;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Events
{
    public class BleAdapterStateChangedEvent : Prism.Events.PubSubEvent<AccessState>
    {
    }
}
