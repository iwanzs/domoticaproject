using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarduinoApp.Models
{
    class Results
    {
        [PrimaryKey, AutoIncrement, Unique, NotNull]
        public int ID { get; set; }
        [NotNull]
        public int ProfileID { get; set; }
        [NotNull]
        public string Value { get; set; }
        [NotNull]
        public int Date { get; set; }
    }
}
