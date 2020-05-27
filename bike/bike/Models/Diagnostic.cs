using SQLite;
using SQLiteNetExtensions.Attributes;
using System;


namespace bike.Models
{
    public partial class Diagnostic
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        [ForeignKey(typeof(FaultType))]
        public int FaultTypeId { get; set; }
        [ManyToOne]
        public FaultType FaultType { get; set; }
    }
}
