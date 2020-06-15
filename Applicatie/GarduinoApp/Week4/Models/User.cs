using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Week4.Models
{
    class User
    {
        [PrimaryKey, AutoIncrement, Unique, NotNull]
        public int UserID { get; set; }
        [Unique, NotNull]
        public string Username { get; set; }
        [NotNull]
        public string Password { get; set; }
    }
}

