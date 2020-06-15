using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;

namespace Week4.Models
{
    class MovieUser
    {
        [NotNull]
        public int UserID { get; set; }
        [NotNull]
        public int MovieID { get; set; }
    }
}
