using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using WeatherApp.Data;
using WeatherApp.Config;

namespace WeatherApp.Services
{
    /// <summary>
    /// Modern API client for fetching weather data
    /// Students will complete the implementation following async/await patterns
    /// </summary>
    public class WeatherApiClient : MonoBehaviour
    {
        [Header("API Configuration")]
        [SerializeField] private string baseUrl = "http://api.openweathermap.org/data/2.5/weather";

        /// <summary>
        /// Fetch weather data for a specific city using async/await pattern
        /// TODO: Students will implement this method
        /// </summary>
        /// <param name="city">City name to get weather for</param>
        /// <returns>WeatherData object or null if failed</returns>
        public async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                Debug.LogError("City name cannot be empty");
                return null;
            }

            if (!ApiConfig.IsApiKeyConfigured())
            {
                Debug.LogError("API key not configured. Please set up your config.json file in StreamingAssets folder.");
                return null;
            }

            string url = $"{baseUrl}?q={city}&appid={ApiConfig.OpenWeatherMapApiKey}&units=metric";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("API request error: " + request.error);
                    return null;
                }

                try
                {
                    string json = request.downloadHandler.text;
                    Debug.Log("RAW JSON from API:\n" + json);  // <-- Lägg till detta

                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(json);
                    return weatherData;
                }
                catch (Exception ex)
                {
                    Debug.LogError("JSON deserialization error: " + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Example usage method - students can use this as reference
        /// </summary>
        private async void Start()
        {
            // Example: Get weather for London
            var weatherData = await GetWeatherDataAsync("London");
            
            if (weatherData != null && weatherData.IsValid)
            {
                Debug.Log($"Weather in {weatherData.CityName}: {weatherData.TemperatureInCelsius:F1}°C");
                Debug.Log($"Description: {weatherData.PrimaryDescription}");
            }
            else
            {
                Debug.LogError("Failed to get weather data");
            }
        }
    }
}