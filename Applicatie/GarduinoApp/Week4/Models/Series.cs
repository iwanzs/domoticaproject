using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Week4.Models
{
    class Series
    {
        [PrimaryKey, AutoIncrement, Unique, NotNull]
        public int SeriesID { get; set; }
        [Unique, NotNull]
        public string Title { get; set; }
        public string Description { get; set; }
        [NotNull]
        public int YearStarted { get; set; }
        public int YearEnded { get; set; }
        [NotNull]
        public int NumberOfEpisodes { get; set; }
        [NotNull]
        public int NumberOfSeasons { get; set; }
    }
}
