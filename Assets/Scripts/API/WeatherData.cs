using Newtonsoft.Json;

namespace WeatherApp.Data
{
    public class WeatherData
    {
        [JsonProperty("name")]
        public string CityName { get; set; }

        [JsonProperty("main")]
        public MainData Main { get; set; }

        [JsonProperty("weather")]
        public WeatherDescription[] Weather { get; set; }

        // Returnerar temperatur i Celsius
        public float TemperatureInCelsius => Main?.Temp ?? 0f;

        // Returnerar första väderbeskrivningen
        public string PrimaryDescription => Weather != null && Weather.Length > 0
            ? Weather[0].Description
            : "No description";

        // Används för att kontrollera om datan är giltig
        public bool IsValid => !string.IsNullOrEmpty(CityName) && Weather != null && Main != null;
    }

    public class MainData
    {
        [JsonProperty("temp")]
        public float Temp { get; set; }
    }

    public class WeatherDescription
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}