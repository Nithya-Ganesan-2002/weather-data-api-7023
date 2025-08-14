using System.ComponentModel.DataAnnotations;

namespace WeatherApi.Models
{
    /// <summary>
    /// Query parameters for the weather forecast endpoint.
    /// </summary>
    // PUBLIC_INTERFACE
    public class WeatherQuery
    {
        /// <summary>
        /// City name to fetch the forecast for. Mock data uses this to seed deterministic results.
        /// </summary>
        public string? City { get; set; } = "Springfield";

        /// <summary>
        /// Number of forecast days requested. If not provided, default is loaded from configuration.
        /// </summary>
        [Range(1, 14)]
        public int? Days { get; set; }

        /// <summary>
        /// Units system: "metric" (Celsius) or "imperial" (Fahrenheit). If not provided, defaults from configuration.
        /// </summary>
        public string? Units { get; set; }
    }

    /// <summary>
    /// The API response containing forecast details for a city.
    /// </summary>
    // PUBLIC_INTERFACE
    public class WeatherResponse
    {
        /// <summary>
        /// City name used for the forecast.
        /// </summary>
        public required string City { get; set; }

        /// <summary>
        /// Units used for temperature values in the forecast ("metric" or "imperial").
        /// </summary>
        public required string Units { get; set; }

        /// <summary>
        /// Number of days included in the forecast.
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// The list of daily forecasts.
        /// </summary>
        public List<WeatherDailyForecast> Forecast { get; set; } = new();
    }

    /// <summary>
    /// One day's weather forecast values.
    /// </summary>
    // PUBLIC_INTERFACE
    public class WeatherDailyForecast
    {
        /// <summary>
        /// The date of the forecast entry (UTC).
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Temperature value in the requested units (C for metric, F for imperial).
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Units label for the temperature value ("C" or "F").
        /// </summary>
        public required string Unit { get; set; }

        /// <summary>
        /// A short textual summary of the expected conditions.
        /// </summary>
        public required string Summary { get; set; }

        /// <summary>
        /// Approximate relative humidity percentage (0-100).
        /// </summary>
        public int Humidity { get; set; }

        /// <summary>
        /// Approximate wind speed in the requested units (kph for metric, mph for imperial).
        /// </summary>
        public double WindSpeed { get; set; }
    }
}
