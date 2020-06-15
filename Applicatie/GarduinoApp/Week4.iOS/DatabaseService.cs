using SQLite;
using System;
using System.IO;
using Foundation;
using Xamarin.Forms;
using Week4.iOS;
using Xamarin.Forms.PlatformConfiguration;

[assembly: Dependency(typeof(DatabaseService))]

namespace Week4.iOS
{
    public class DatabaseService : IDBInterface
    {
        public SQLiteConnection CreateConnection()
        {
            var sqliteFilename = "MovieDB.db";

            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }
            string path = Path.Combine(libFolder, sqliteFilename);

            if (!File.Exists(path))
            {
                var existingDb = NSBundle.MainBundle.PathForResource("MovieDatabase", "db");
                File.Copy(existingDb, path);
            }

            var connection = new SQLiteConnection(path, false);

            return connection;
        }
    }
}