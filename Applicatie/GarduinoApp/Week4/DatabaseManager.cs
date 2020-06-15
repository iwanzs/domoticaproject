using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.XPath;
using SQLite;
using Week4.Models;
using Xamarin.Forms;


namespace Week4
{
    class DatabaseManager
    {

        SQLiteConnection dbConnection = DependencyService.Get<IDBInterface>().CreateConnection();

        public DatabaseManager()
        {

        }

        public List<Movie> GetAllMovies()
        {
            return new List<Movie>(dbConnection.Query<Movie>("SELECT * FROM [Movie]"));
        }

        public List<Series> GetAllSeries()
        {
            return new List<Series>(dbConnection.Query<Series>("SELECT * FROM [Series]"));
        }

        public List<Movie> GetAllMoviesByUser()
        {
            try
            {
                int currentID = Configuration.UserID;
                Debug.WriteLine("De debug waarde is:" + currentID);
                List<MovieUser> geralt = new List<MovieUser>(dbConnection.Query<MovieUser>("SELECT * FROM [MovieUser] WHERE UserID = '" + currentID + "'"));

                List<Movie> movies = new List<Movie>();
                foreach (MovieUser movie in geralt)
                {
                    movies.Add(dbConnection.FindWithQuery<Movie>("SELECT * FROM [Movie] WHERE MovieID = '" + movie.MovieID + "'"));
                    Debug.WriteLine("De movies waarde op deze index:" + movie.MovieID);
                }
                return movies;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine("Probleemgeval Movies = " + e);
                return new List<Movie>();
            }
        }

        public List<Series> GetAllSeriesByUser()
        {
            try
            {
                int currentID = Configuration.UserID;
                //Debug.WriteLine("De debug waarde is:" + currentID);
                List<SeriesUser> geralt = new List<SeriesUser>(dbConnection.Query<SeriesUser>("SELECT * FROM [SeriesUser] WHERE UserID = '" + currentID + "'"));

                List<Series> series = new List<Series>();
                foreach (SeriesUser serie in geralt)
                {
                    series.Add(dbConnection.FindWithQuery<Series>("SELECT * FROM [Series] WHERE SeriesID = '" + serie.SeriesID + "'"));
                    // Debug.WriteLine("De movies waarde op deze index:" + serie.SeriesID);
                }
                return series;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine("Probleemgeval Movies = " + e);
                return new List<Series>();
            }
        }

        public bool DoesAccountExist(string username, string password)
        {
            return dbConnection.FindWithQuery<User>("SELECT * FROM [User] WHERE Username='" + username + "' AND Password='" + password + "'") != null;
        }

        public bool AddUser(string username, string password)
        {
            if (!DoesAccountExist(username, password))
            {
                dbConnection.Insert(new User { Username = username, Password = password }); ;
                return true;
            }
            else
            {
                return false;
            }
        }

        public User GetUser()
        {
            return dbConnection.FindWithQuery<User>("SELECT * FROM User WHERE Username='" + Configuration.Username + "'");
        }

        public void AddMovie(string title, string description, int year)
        {
            dbConnection.Insert(new Movie { Title = title, Description = description, Year = year });
        }

        public void AddFavoriteMovie(int userID, int movieID)
        {
            dbConnection.Insert(new MovieUser { UserID = userID, MovieID = movieID });
        }

        public void AddSeries(string title, string description, int yearStarted, int yearEnded, int numberOfEpisodes, int numberOfSeasons)
        {
            dbConnection.Insert(new Series { Title = title, Description = description, YearStarted = yearStarted, YearEnded = yearEnded, NumberOfEpisodes = numberOfEpisodes, NumberOfSeasons = numberOfSeasons });
        }

        public void AddFavoriteSeries(int userID, int seriesID)
        {
            dbConnection.Insert(new SeriesUser { UserID = userID, SeriesID = seriesID });
        }

        public void DeleteFavoriteMovie(int userID, int movieID)
        {
            dbConnection.Query<MovieUser>("DELETE FROM [MovieUser] Where UserID = '" + userID + "' AND MovieID ='" + movieID + "'");
        }

        public void DeleteFavoriteSeries(int userID, int seriesID)
        {
            dbConnection.Query<SeriesUser>("DELETE FROM [SeriesUser] Where UserID = '" + userID + "' AND SeriesID ='" + seriesID + "'");
        }
    }
}
