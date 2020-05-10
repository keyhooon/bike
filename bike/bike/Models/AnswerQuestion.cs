using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Models
{
    public class AnswerQuestion
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Category { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string Detail { get; set; }

    }
}
