using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using Week4.Models;
using GarduinoApp.Models;

namespace Week4
{
    public static class Configuration
    {
        public static string Username { get; set; }

        public static int UserID { get; set; }
        public static int ProfileID { get; set; }

        public static List<Connection> ConnectionsList = new List<Connection>();

        public static void AddConnection(int ID, Socket socket)
        {
            Connection con = new Connection(ID, socket);
            ConnectionsList.Add(con);
        }

        public static Connection GetConnection(int ID)
        {
            Connection config = ConnectionsList.Find(item => item.ConnectionID == ID);
            return config;
        }

        public static void RemoveConnection(int ID)
        {
            Connection config = ConnectionsList.Find(item => item.ConnectionID == ID);
            if(config.ConnectionSocket != null)
            {
                config.ConnectionSocket.Close();
            }
            ConnectionsList.RemoveAll(item => item.ConnectionID == ID);
        }

        public static void RemoveAllConnections()
        {
            foreach (var con in ConnectionsList)
            {
                Connection config = ConnectionsList.Find(item => item.ConnectionID == con.ConnectionID);
                config.ConnectionSocket.Close();
            }
            ConnectionsList.Clear();
        }

        //Connection Endpoint for the API
        public static string OpenWeatherMapEndpoint = "https://api.openweathermap.org/data/2.5/weather";
        //API Key to connect with the API
        public static string OpenWeatherMapAPIKey = "a50e98f4a2873317139fba8740468d4c";
    }

}

