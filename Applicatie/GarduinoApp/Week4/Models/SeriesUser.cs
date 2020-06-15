using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Week4.Models
{
    class SeriesUser
    {
        [NotNull]
        public int UserID { get; set; }
        [NotNull]
        public int SeriesID { get; set; }
    }
}
