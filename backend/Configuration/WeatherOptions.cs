namespace WeatherApi.Configuration
{
    /// <summary>
    /// Options for weather service defaults that can be configured via environment variables.
    /// WEATHER_DEFAULT_UNITS: "metric" or "imperial" (default: metric).
    /// WEATHER_FORECAST_DAYS: integer number of days to return for forecasts (default: 5, clamped 1..14).
    /// </summary>
    // PUBLIC_INTERFACE
    public class WeatherOptions
    {
        /// <summary>
        /// Default units for temperature ("metric" => Celsius, "imperial" => Fahrenheit).
        /// </summary>
        public string DefaultUnits { get; set; } = "metric";

        /// <summary>
        /// Default number of forecast days to return.
        /// </summary>
        public int DefaultForecastDays { get; set; } = 5;
    }
}
