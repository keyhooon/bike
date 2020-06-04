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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Integrations.Sqlite;
using BruTile.Wms;

namespace bike.Shiny
{
    public class Startup : PrismStartup
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            Log.UseConsole();
            Log.UseDebug();

            services.UseCache();
            services.UseLogging();
            services.UseUserDialog();
            services.UseGps<GpsDelegate>();
            services.UseBleCentral();
            services.UseSqliteSettings();
            // Register Stuff
        }


    }
    public static class LoggingExtension
    {
        public static void UseLogging(this IServiceCollection services, bool enableCrashes = true, bool enableEvents = false)
        {
            services.TryAddSingleton<SqliteConnection>();
            services.RegisterPostBuildAction(sp =>
            {
                var conn = sp.GetService<SqliteConnection>();
                var serializer = sp.GetService<ISerializer>();
                Log.AddLogger(new SqliteLog(conn, serializer), enableCrashes, enableEvents);
                conn.Seed();
            });
        }
        public static void UseCache(this IServiceCollection services) => services.UseMemoryCache();

        public static void UseUserDialog(this IServiceCollection services) => services.AddSingleton<IUserDialogs, UserDialogs>();

    }

}
