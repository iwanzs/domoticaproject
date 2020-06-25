using GarduinoApp.Models;
using GarduinoApp.Models.Weather;
using System;
using Week4;
using Xamarin.Forms;

namespace GarduinoApp.Views
{
    public partial class CurrentWeatherPage : ContentPage
    {
        RestService _restService;
        public string Location;
        DatabaseManager databasemanager;

        /// <summary>
        /// Starts the connection to the RestService (HTTP Request to API) and initializes the page.
        /// </summary>
        public CurrentWeatherPage()
        {
            InitializeComponent();

            databasemanager = new DatabaseManager();

            _restService = new RestService();
            Profiles currentProfile = databasemanager.GetProfileInformation(Configuration.ProfileID);
            Location = currentProfile.Location;
            GetWeather();
        }
        /// <summary>
        /// Sends the request to the API based on the city the user put in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void GetWeather()
        {
            if (!string.IsNullOrWhiteSpace(Location))
            {
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUri(Configuration.OpenWeatherMapEndpoint));
                weatherData.Main.Temperature = weatherData.Main.Temperature + " °F";
                weatherData.Main.Humidity = weatherData.Main.Humidity + "%";
                weatherData.Wind.Speed = weatherData.Wind.Speed + " m/s";
                weatherData.Main.dateTime = DateTime.UtcNow;
                weatherData.Main.Pressure = weatherData.Main.Pressure + " hpa";
                BindingContext = weatherData;
            }
        }
        /// <summary>
        /// Generates the URI/URL to send to the Weather API, this retrieves the information.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?q={Location}";
            requestUri += "&units=imperial"; // or units=metric
            requestUri += $"&APPID={Configuration.OpenWeatherMapAPIKey}";
            return requestUri;
        }

        private void RefreshWeather(object sender, EventArgs e)
        {
            GetWeather();
        }
    }
}