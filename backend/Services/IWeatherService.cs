using WeatherApi.Models;

namespace WeatherApi.Services
{
    /// <summary>
    /// Service interface for obtaining weather forecasts.
    /// </summary>
    // PUBLIC_INTERFACE
    public interface IWeatherService
    {
        /// <summary>
        /// Generates a mock weather forecast using deterministic randomness based on input parameters.
        /// </summary>
        /// <param name="city">City to generate the forecast for.</param>
        /// <param name="days">Number of days to include.</param>
        /// <param name="units">Units system ("metric" or "imperial").</param>
        /// <returns>A response containing daily forecast entries.</returns>
        WeatherResponse GetForecast(string city, int days, string units);
    }
}
