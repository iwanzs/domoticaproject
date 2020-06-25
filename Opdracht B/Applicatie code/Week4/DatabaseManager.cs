﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.XPath;
using GarduinoApp.Models;
using SQLite;
using Week4.Models;
using Xamarin.Forms;


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

        public void AddProfile(string Name, string Location, int Threshold, string IP, string Port, int ArduinoPinNumber)
        {
            Profiles profile = new Profiles() { UserID = Configuration.UserID, Name = Name, Location = Location, Threshold = Threshold, IP = IP, Port = Port, ArduinoPinNumber = ArduinoPinNumber };
            dbConnection.Insert(profile);
        }

        public void EditProfile(int ID, string Name, string Location, int Threshold, string IP, string Port, int ArduinoPinNumber)
        {
            dbConnection.Execute("UPDATE [Profiles] SET Name = ?, Location = ?, Threshold = ?, IP = ?, Port = ?, ArduinoPinNumber = ? WHERE ID = ? AND UserID = ? ", Name, Location, Threshold, IP, Port, ArduinoPinNumber, ID, Configuration.UserID);
        }

        public void DeleteProfile(int ID, int UserID)
        {
            dbConnection.Execute("DELETE FROM [Profiles] WHERE ID = ? AND UserID = ? ", ID, UserID);
        }

        public List<Profiles> GetProfiles()
        {
            return dbConnection.Query<Profiles>("SELECT * FROM [Profiles] WHERE UserID='" + Configuration.UserID + "'");
        }

        public Profiles GetProfileInformation(int ID)
        {
            Profiles profile = dbConnection.FindWithQuery<Profiles>("SELECT * FROM [Profiles] WHERE ID = '" + ID + "'");
            return profile;
        }

        public void SetThreshold(int sldrTempValue, int ID)
        {
            dbConnection.Execute("UPDATE [Profiles] SET Threshold = ? WHERE ID = ? ", sldrTempValue, ID);
        }

        public void SetAutoEnabled(int Enable, int ID)
        {
            dbConnection.Execute("UPDATE [Profiles] SET AutoEnabled = ? WHERE ID = ? ", Enable, ID);
        }

        public Results GetLatestResult(int id)
        {
            List<Results> results = dbConnection.Query<Results>("SELECT Value, Date FROM [Results] WHERE ProfileID = ?", id);

            if (results.Count != 0)
            {
                Results result = results[results.Count -1];
                return result;
            }

            //return een lege
            return new Results();
        }

        public List<Results> GetPeriodOfResults(int id)
        {
            List<Results> query = dbConnection.Query<Results>("SELECT Value, Date FROM [Results] WHERE ProfileID = ? ORDER BY Date DESC", id);
            List<Results> result = new List<Results>();

            //reverse so the latest objects are on top
            query.Reverse();

            //amount of datapoints to put in the graph
            int amount = 60;

            if (query.Count < amount)
            {
                amount = query.Count;
            }

            for (int i = 0; i < amount; i++)
            {
                result.Add(query[i]);
            }

            return result;
        }

        public void AddResult(int id, int value, string date)
        {
            dbConnection.Execute("INSERT INTO [Results] (ProfileID, Value, Date) VALUES (?,?,?)", id, value, date);
        }
    }
}
