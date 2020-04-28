using bike.Models;
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
            conn.CreateTable<AnswerQuestion>();
            conn.CreateTable<AppStateEvent>();
            conn.CreateTable<BleAdapterState>();
            conn.CreateTable<BleConnectedPeripheral>();
            conn.CreateTable<GpsData>();
        }
        public AsyncTableQuery<AnswerQuestion> AnswerQuestion => Table<AnswerQuestion>();
        public AsyncTableQuery<AppStateEvent> AppStateEvent => Table<AppStateEvent>();
        public AsyncTableQuery<BleAdapterState> BleAdapterState => Table<BleAdapterState>();
        public AsyncTableQuery<BleConnectedPeripheral> BleConnectedPeripheral => Table<BleConnectedPeripheral>();
        public AsyncTableQuery<GpsData> GpsData => Table<GpsData>();
    }
}
