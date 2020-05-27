using bike.Models;
using bike.Models.ContactUs;
using bike.Models.Logging;
using Shiny.Integrations.Sqlite;
using Shiny.IO;
using SQLite;
using System;

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
            conn.CreateTable<FaultType>();
            conn.CreateTable<Diagnostic>();

            conn.CreateTable<Trip>();
            conn.CreateTable<TripDetail>();
        }
        public AsyncTableQuery<AnswerQuestion> AnswerQuestions => Table<AnswerQuestion>();
        public AsyncTableQuery<AppStateEvent> AppStateEvent => Table<AppStateEvent>();

        public AsyncTableQuery<Building> Buildings => Table<Building>();
        public AsyncTableQuery<Diagnostic> Diagnostics => Table<Diagnostic>();
        public AsyncTableQuery<FaultType> FaultType => Table<FaultType>();

        public AsyncTableQuery<Trip> Trips => Table<Trip>();
        public AsyncTableQuery<TripDetail> TripDetails => Table<TripDetail>();


        public void Seed()
        {
            GetConnection().DeleteAll<AnswerQuestion>();
            GetConnection().DeleteAll<Building>();
            GetConnection().DeleteAll<FaultType>();
            GetConnection().DeleteAll<Diagnostic>();

            GetConnection().InsertAll(new[] {
                new AnswerQuestion() {
                    Category = "Primary Question",
                    Question = "How to work with this program?",
                    Answer="You can Work this program what you want to do.",
                    Detail= "Its Easy to use, use must first read Instruction before use it. have fun."},
                new AnswerQuestion() {
                    Category = "Thecnical Question",
                    Question = "how can we Repair my bicycle?",
                    Answer="You should bring your bicycle to repair workshop that specified in web site .",
                    Detail= "You must dont open hardware yourself by own."},
                new AnswerQuestion() {
                    Category = "Primary Question",
                    Question = "How to buy Electrical Bicycle?",
                    Answer="You can contact us to help you where place you must go to buy one Electrical bicycle .",
                    Detail= "We have many store on the city that you can go there to buy a Electrical bicycle, if you want know where are these watch company web site or contact us"},
                        });

            GetConnection().InsertAll(new[] {
                new Building() {
                    Header = "Central Office",
                    Type = BuildingType.Office,
                    Latitude = 35.715298,
                    Longitude = 51.404343,
                    Address = "Tehran, Towhid Sqaure, Sattar Khan Avenue, Kokab Street, No.28",
                    PhoneNumber = "021-66932063",
                    Email = "Keyhanbabazadeh@gmail.com"
                    },
                new Building() {
                    Header = "Workshop1",
                    Type = BuildingType.Office,
                    Latitude = 35.915298,
                    Longitude = 51.304343,
                    Address = "Tehran, Towhid Sqaure, Sattar Khan Avenue, Kokab Street, No.28",
                    PhoneNumber = "021-66932063",
                    Email = "Keyhanbabazadeh@gmail.com"
                    },
            });


            GetConnection().InsertAll(new[] {
                            new FaultType(){Id = 0, Name = "OverCurrent",Description = "Current exceeded from value that cause damage to bike"},
                new FaultType(){Id = 1, Name = "OverTemprature",Description = "Temprature exceeded from value that cause damage to bike"},
                new FaultType(){Id = 2, Name = "PedalSensor",Description = "Pedal sensor doesn't work properly"},
                new FaultType(){Id = 3, Name = "Throttle",Description = "Throttle doesn't work properly"},
                new FaultType(){Id = 4, Name = "OverVoltage",Description = "Voltage exceeded from value that cause damage to bike"},
                new FaultType(){Id = 5, Name = "UnderVoltage",Description = "Power Drop Down from value that cause damage to bike"},
                new FaultType(){Id = 6, Name = "Motor",Description = "Motor sensor doesn't work properly"},
                new FaultType(){Id = 7, Name = "Drive",Description = "Drive doesn't work properly"} });
            var _faultTypes = FaultType.ToListAsync().Result;
            GetConnection().InsertAll(new[]
            {
                new Diagnostic(){FaultTypeId = _faultTypes[1].Id, StartTime= DateTime.Now - TimeSpan.FromHours(2)},
                new Diagnostic(){FaultTypeId = _faultTypes[3].Id, StartTime= DateTime.Now - TimeSpan.FromDays(2)},
                new Diagnostic(){FaultTypeId = _faultTypes[2].Id, StartTime= DateTime.Now - TimeSpan.FromDays(20), StopTime = DateTime.Now - TimeSpan.FromDays(20) + TimeSpan.FromHours(100)},
                new Diagnostic(){FaultTypeId = _faultTypes[3].Id, StartTime= DateTime.Now - TimeSpan.FromDays(100), StopTime = DateTime.Now - TimeSpan.FromDays(100) + TimeSpan.FromDays(50)},
                new Diagnostic(){FaultTypeId = _faultTypes[4].Id, StartTime= DateTime.Now - TimeSpan.FromDays(200), StopTime = DateTime.Now - TimeSpan.FromDays(200) + TimeSpan.FromDays(148)},
            });

        }

    }
}
