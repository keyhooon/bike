using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shiny.Locations;

namespace bike.Shiny.Delegate
{
    public class GpsDelegate : IGpsDelegate
    {
        public GpsDelegate(SqliteConnection connection)
        {

        }
        public Task OnReading(IGpsReading reading)
        {
            connection
            return Task.CompletedTask;
        }
    }
}
