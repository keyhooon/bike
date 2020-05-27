using SQLite;

namespace bike.Models
{
        public class FaultType
        {
            [PrimaryKey]
            [AutoIncrement]
            public int Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public string ImageAddress{ get; set; }
        }

}
