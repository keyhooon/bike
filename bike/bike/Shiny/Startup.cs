using Microsoft.Extensions.DependencyInjection;
using Shiny.Prism;
using Shiny;
using bike.Shiny.Delegate;
using Shiny.Logging;
using Acr.UserDialogs.Forms;

namespace bike.Shiny
{
    public class Startup : PrismStartup
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            //Log.UseConsole();
            Log.UseDebug();
            services.AddSingleton<SqliteConnection>();
            services.UseMemoryCache();
            services.UseUserDialog();
            services.UseGps<GpsDelegate>();
            services.UseSqliteLogging(true, true);
            // Register Stuff
        }


    }
    public static class LoggingExtension
    {

        public static void UseUserDialog(this IServiceCollection services) => services.AddSingleton<IUserDialogs, UserDialogs>();

    }

}
