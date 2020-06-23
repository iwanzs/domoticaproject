using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.XPath;
using GarduinoApp.Models;
using SQLite;
using Week4.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Week4
{
    class DatabaseManager
    {

        SQLiteConnection dbConnection;

        public DatabaseManager()
        {
            dbConnection = DependencyService.Get<IDBInterface>().CreateConnection();
        }

        public bool DoesAccountExist(string username, string password)
        {
            List<Users> userdata = dbConnection.Query<Users>("SELECT * FROM [Users] WHERE Username = ? ", username);

            if (userdata.Count == 0)
                return false;

            if (userdata[0].Password != password)
                return false;

            return true;
        }

        public bool AddUser(string username, string password)
        {
            List<Users> userdata = dbConnection.Query<Users>("SELECT * FROM [Users] WHERE Username = ? ", username);
            if (userdata.Count == 1)
                return false;
            else
            {
                Users user = new Users() { Username = username, Password = password, Date = (DateTime.Now).ToString() };
                dbConnection.Insert(user);
                return true;
            }
        }

        public Users GetUser()
        {
            try
            {
                return dbConnection.FindWithQuery<Users>("SELECT * FROM [Users] WHERE Username='" + Configuration.Username + "'");
            }
            catch (SQLiteException e)
            {
                Console.WriteLine("Something goes wrong: " + e);
                return new Users();
            }
        }

        public void AddProfile(string Name, int Threshold, string IP, string Port, int ArduinoPinNumber)
        {
            Profiles profile = new Profiles() { UserID = Configuration.UserID, Name = Name, Threshold = Threshold, IP = IP, Port = Port, ArduinoPinNumber = ArduinoPinNumber };
            dbConnection.Insert(profile);
        }

        public void EditProfile(int ID, string Name, int Threshold, string IP, string Port, int ArduinoPinNumber)
        {
            dbConnection.Execute("UPDATE [Profiles] SET Name = ?, Threshold = ?, IP = ?, Port = ?, ArduinoPinNumber = ? WHERE ID = ? AND UserID = ? ", Name, Threshold, IP, Port, ArduinoPinNumber, ID, Configuration.UserID);
        }

        public void DeleteProfile(int ID, int UserID)
        {
            dbConnection.Execute("DELETE FROM [Profiles] WHERE ID = ? AND UserID = ? ", ID, UserID);
        }

        public List<Profiles> GetProfiles()
        {
            return dbConnection.Query<Profiles>("SELECT * FROM [Profiles] WHERE UserID='" + Configuration.UserID + "'");
        }

<<<<<<< HEAD
        public Profiles GetProfileInformation(int ID)
        {
            Profiles profile = dbConnection.FindWithQuery<Profiles>("SELECT * FROM [Profiles] WHERE ID = ? ", ID);
            return profile;
=======
        public List<Results> GetGraphData()
        {
            //id of the profile - only get data for that profile
            //current date so it gets all values until today/now
            //the backlog to what you want to get values
            return dbConnection.Query<Results>("SELECT * FROM [Results] WHERE ProfileID = ? AND Date <= ? AND Date >= ?");
>>>>>>> nando
        }
    }
}
