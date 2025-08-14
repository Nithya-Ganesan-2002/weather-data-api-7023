using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherApi.Configuration;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Endpoints
{
    /// <summary>
    /// Extension methods to register weather-related endpoints.
    /// </summary>
    // PUBLIC_INTERFACE
    public static class WeatherEndpoints
    {
        /// <summary>
        /// Registers the weather endpoints to the application's route builder.
        /// </summary>
        /// <param name="routes">The application's endpoint route builder.</param>
        // PUBLIC_INTERFACE
        public static void MapWeatherEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("")
                              .WithTags("Weather");

            // GET /weather
            group.MapGet("/weather", GetWeather)
                 .WithName("GetWeather")
                 .Produces<WeatherResponse>(StatusCodes.Status200OK)
                 .ProducesProblem(StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// Handler for the /weather endpoint using query parameters.
        /// </summary>
        /// <param name="service">Injected weather service.</param>
        /// <param name="query">Query object populated from the request.</param>
        /// <param name="options">Default options populated from configuration.</param>
        /// <returns>Weather forecast response.</returns>
        private static IResult GetWeather(
            [FromServices] IWeatherService service,
            [AsParameters] WeatherQuery query,
            IOptions<WeatherOptions> options)
        {
            var cfg = options.Value;

            var city = string.IsNullOrWhiteSpace(query.City) ? "Springfield" : query.City.Trim();
            var days = query.Days ?? cfg.DefaultForecastDays;
            days = Math.Clamp(days, 1, 14);

            var units = string.IsNullOrWhiteSpace(query.Units) ? cfg.DefaultUnits : query.Units.Trim();
            units = units.Equals("imperial", StringComparison.OrdinalIgnoreCase) ? "imperial" : "metric";

            var result = service.GetForecast(city, days, units);
            return Results.Ok(result);
        }
    }
}
