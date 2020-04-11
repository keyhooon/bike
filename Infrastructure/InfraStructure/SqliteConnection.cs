
using Shiny.IO;
using SQLite;
using System.IO;

namespace Infrastructure
{
    public class MedicalMobileSqliteConnection : SQLiteAsyncConnection
    {
        public MedicalMobileSqliteConnection(IFileSystem fileSystem) : base(Path.Combine(fileSystem.AppData.FullName, "sample.db"))
        {
            var conn = GetConnection();
            //conn.CreateTable<AppStateEvent>();
            //conn.CreateTable<BeaconEvent>();
            //conn.CreateTable<GeofenceEvent>();
            //conn.CreateTable<JobLog>();
            //conn.CreateTable<BleEvent>();
            //conn.CreateTable<GpsEvent>();
            //conn.CreateTable<HttpEvent>();
            //conn.CreateTable<NotificationEvent>();
            //conn.CreateTable<PushEvent>();
        }

        //public AsyncTableQuery<AppStateEvent> AppStateEvents => Table<AppStateEvent>();
        //public AsyncTableQuery<BeaconEvent> BeaconEvents => Table<BeaconEvent>();
        //public AsyncTableQuery<BleEvent> BleEvents => Table<BleEvent>();
        //public AsyncTableQuery<GeofenceEvent> GeofenceEvents => Table<GeofenceEvent>();
        //public AsyncTableQuery<JobLog> JobLogs => Table<JobLog>();
        //public AsyncTableQuery<GpsEvent> GpsEvents => Table<GpsEvent>();
        //public AsyncTableQuery<HttpEvent> HttpEvents => Table<HttpEvent>();
        //public AsyncTableQuery<NotificationEvent> NotificationEvents => Table<NotificationEvent>();
        //public AsyncTableQuery<PushEvent> PushEvents => Table<PushEvent>();
    }
}
