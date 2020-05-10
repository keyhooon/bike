using bike.Models;
using bike.Models.ContactUs;
using bike.Models.Logging;
using Shiny.Integrations.Sqlite;
using Shiny.IO;
using SQLite;

namespace bike
{
    public class SqliteConnection : ShinySqliteConnection
    {
        public SqliteConnection(IFileSystem fileSystem) : base(fileSystem)
        {
            var conn = this.GetConnection();
            conn.CreateTable<AnswerQuestion>();
            conn.CreateTable<AppStateEvent>();
            conn.CreateTable<BleAdapterState>();
            conn.CreateTable<BleConnectedPeripheral>();
            conn.CreateTable<GpsData>();
            conn.CreateTable<Building>();
        }
        public AsyncTableQuery<AnswerQuestion> AnswerQuestions => Table<AnswerQuestion>();
        public AsyncTableQuery<AppStateEvent> AppStateEvent => Table<AppStateEvent>();
        public AsyncTableQuery<Building> Buildings => Table<Building>();
        public AsyncTableQuery<Trip> Trips => Table<Trip>();
        public AsyncTableQuery<TripDetail> TripDetails => Table<TripDetail>();


    }
}
