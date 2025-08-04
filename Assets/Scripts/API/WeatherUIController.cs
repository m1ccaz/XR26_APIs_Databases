using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WeatherApp.Services;
using WeatherApp.Data;

namespace WeatherApp.UI
{
    /// <summary>
    /// UI Controller for the Weather Application
    /// Handles input, displays weather data and communicates with the API client
    /// </summary>
    public class WeatherUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField cityInputField;
        [SerializeField] private Button getWeatherButton;
        [SerializeField] private TextMeshProUGUI weatherDisplayText;
        [SerializeField] private TextMeshProUGUI statusText;

        [Header("API Client")]
        [SerializeField] private WeatherApiClient apiClient;

        private void Start()
        {
            // Set up button click listener
            getWeatherButton.onClick.AddListener(OnGetWeatherClicked);

            // Initialize UI state
            SetStatusText("Enter a city name and click Get Weather");
        }

        /// <summary>
        /// Called when user clicks the Get Weather button
        /// </summary>
        private async void OnGetWeatherClicked()
        {
            // Get city name from input field
            string cityName = cityInputField.text;

            // Validate input
            if (string.IsNullOrWhiteSpace(cityName))
            {
                SetStatusText("Please enter a city name");
                return;
            }

            // Disable button and show loading state
            getWeatherButton.interactable = false;
            SetStatusText("Loading weather data...");
            weatherDisplayText.text = "";

            try
            {
                // Get weather data from API
                var weatherData = await apiClient.GetWeatherDataAsync(cityName);

                if (weatherData != null && weatherData.IsValid)
                {
                    DisplayWeatherData(weatherData);
                    SetStatusText("Weather data loaded successfully!");
                }
                else
                {
                    SetStatusText("Failed to retrieve weather data.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error getting weather data: {ex.Message}");
                SetStatusText("An error occurred. Please try again.");
            }
            finally
            {
                // Re-enable button
                getWeatherButton.interactable = true;
            }
        }

        /// <summary>
        /// Displays the weather information in the UI
        /// </summary>
        private void DisplayWeatherData(WeatherData weatherData)
        {
            string displayText = $"City: {weatherData.CityName}\n" +
                                 $"Temperature: {weatherData.TemperatureInCelsius:F1}°C\n" +
                                 $"Description: {weatherData.PrimaryDescription}";

            weatherDisplayText.text = displayText;
        }

        /// <summary>
        /// Updates the status text
        /// </summary>
        private void SetStatusText(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }

        /// <summary>
        /// Clears the weather display and resets the input
        /// </summary>
        public void ClearDisplay()
        {
            weatherDisplayText.text = "";
            cityInputField.text = "";
            SetStatusText("Enter a city name and click Get Weather");
        }
    }
}