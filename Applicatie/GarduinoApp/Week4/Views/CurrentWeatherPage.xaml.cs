using GarduinoApp.Models.Weather;
using System;
using Week4;
using Xamarin.Forms;

namespace GarduinoApp.Views
{
    public partial class CurrentWeatherPage : ContentPage
    {
        RestService _restService;

        /// <summary>
        /// Starts the connection to the RestService (HTTP Request to API) and initializes the page.
        /// </summary>
        public CurrentWeatherPage()
        {
            InitializeComponent();
            _restService = new RestService();
        }
        /// <summary>
        /// Sends the request to the API based on the city the user put in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_cityEntry.Text))
            {
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUri(Configuration.OpenWeatherMapEndpoint));
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
            requestUri += $"?q={_cityEntry.Text}";
            requestUri += "&units=imperial"; // or units=metric
            requestUri += $"&APPID={Configuration.OpenWeatherMapAPIKey}";
            return requestUri;
        }

        protected override void OnAppearing()
        {
            //GenerateRequestUri()
        }
    }
}