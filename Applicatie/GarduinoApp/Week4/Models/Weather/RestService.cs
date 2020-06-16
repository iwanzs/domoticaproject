using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GarduinoApp.Models.Weather
{
    /// <summary>
    /// Used to grab the data from the API
    /// </summary>
    public class RestService
    {
        HttpClient _client;

        /// <summary>
        /// Starts a connection for an HTTP Request
        /// </summary>
        public RestService()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Fills the WeatherData model with the required information.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<WeatherData> GetWeatherData(string query)
        {
            WeatherData weatherData = null;
            try
            {
                var response = await _client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\t\tERROR {0}", ex.Message);
            }

            return weatherData;
        }
    }
}

