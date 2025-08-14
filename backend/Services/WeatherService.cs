using WeatherApi.Models;

namespace WeatherApi.Services
{
    /// <summary>
    /// Mock implementation of a weather service that produces deterministic pseudo-random data.
    /// </summary>
    public class WeatherService : IWeatherService
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild",
            "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
            "Rainy", "Cloudy", "Windy", "Foggy", "Sunny"
        };

        /// <inheritdoc />
        public WeatherResponse GetForecast(string city, int days, string units)
        {
            var unitLabel = units.Equals("imperial", StringComparison.OrdinalIgnoreCase) ? "F" : "C";
            var isImperial = unitLabel == "F";

            // Seed random with a stable input (city + days + today) for deterministic output
            var seed = HashToSeed($"{city}:{days}:{DateTime.UtcNow:yyyy-MM-dd}:{units}");
            var rng = new Random(seed);

            var forecasts = new List<WeatherDailyForecast>();
            for (int i = 0; i < days; i++)
            {
                var date = DateTime.UtcNow.Date.AddDays(i + 1);
                var baseTempC = rng.Next(-5, 35) + rng.NextDouble(); // Base temp in C
                var temp = isImperial ? CToF(baseTempC) : baseTempC;

                var humidity = rng.Next(20, 100);
                var wind = isImperial ? Math.Round(rng.NextDouble() * 25 + 1, 1) : Math.Round(rng.NextDouble() * 40 + 1, 1);
                var summary = Summaries[rng.Next(Summaries.Length)];

                forecasts.Add(new WeatherDailyForecast
                {
                    Date = date,
                    Temperature = Math.Round(temp, 1),
                    Unit = unitLabel,
                    Summary = summary,
                    Humidity = humidity,
                    WindSpeed = wind
                });
            }

            return new WeatherResponse
            {
                City = city,
                Units = units.Equals("imperial", StringComparison.OrdinalIgnoreCase) ? "imperial" : "metric",
                Days = days,
                Forecast = forecasts
            };
        }

        private static int HashToSeed(string input)
        {
            unchecked
            {
                int hash = 23;
                foreach (var ch in input)
                {
                    hash = (hash * 31) + ch;
                }
                return hash;
            }
        }

        private static double CToF(double c) => (c * 9.0 / 5.0) + 32.0;
    }
}
