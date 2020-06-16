using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarduinoApp.Models
{
    class Profiles
    {
        [PrimaryKey, AutoIncrement, Unique, NotNull]
        public int ID { get; set; }
        [NotNull]
        public int UserID { get; set; }
        [NotNull]
        public string Name { get; set; }
        [NotNull]
        public int Threshold { get; set; }
        [NotNull]
        public string IP { get; set; }
        [NotNull]
        public string Port { get; set; }
        [NotNull]
        public int ArduinoPinNumber { get; set; }


    }
}
