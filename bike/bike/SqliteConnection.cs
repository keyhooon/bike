using bike.Models.Logging;
using Shiny.IO;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bike
{
    public class SqliteConnection : SQLiteAsyncConnection
    {
        public SqliteConnection(IFileSystem fileSystem) : base(Path.Combine(fileSystem.AppData.FullName, "sqlite.db"))
        {
            var conn = this.GetConnection();
            conn.CreateTable<AppStateEvent>();
            conn.CreateTable<BleEvent>();
            conn.CreateTable<GpsEvent>();
        }

        public AsyncTableQuery<AppStateEvent> AppStateEvents => this.Table<AppStateEvent>();
        public AsyncTableQuery<BleEvent> BleEvents => this.Table<BleEvent>();
        public AsyncTableQuery<GpsEvent> GpsEvents => this.Table<GpsEvent>();
    }
}
