using Microsoft.Extensions.DependencyInjection;
using Shiny.Prism;
using Shiny.Locations;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using bike.Shiny.Delegate;
using Shiny.Logging;
using Acr.UserDialogs.Forms;
using Shiny.Infrastructure;

namespace bike.Shiny
{
    public class Startup : PrismStartup
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            Log.UseConsole();
            Log.UseDebug();
            services.UseMemoryCache();
            services.UseSqliteLogging(true, true);


            services.AddSingleton<SqliteConnection>();
            services.AddSingleton<CoreDelegateServices>();
            services.AddSingleton<IUserDialogs, UserDialogs>();
            services.UseGps<GpsDelegate>();
            services.UseBleCentral();
            services.UseSqliteLogging(true, true);
            services.BuildServiceProvider().GetService<SqliteConnection>().SetBusyTimeoutAsync(TimeSpan.FromSeconds(1));
            // Register Stuff
        }
    }
}
