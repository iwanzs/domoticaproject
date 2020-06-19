using System;
using System.Collections.Generic;
using System.Text;

namespace Week4
{
    public static class Configuration
    {
        public static string Username { get; set; }

        public static int UserID { get; set; }
        public static int ProfileID { get; set; }

        //Connection Endpoint for the API
        public static string OpenWeatherMapEndpoint = "https://api.openweathermap.org/data/2.5/weather";
        //API Key to connect with the API
        public static string OpenWeatherMapAPIKey = "a50e98f4a2873317139fba8740468d4c";
    }

}

