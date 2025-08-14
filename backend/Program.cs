using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using NSwag;
using WeatherApi.Configuration;
using WeatherApi.Endpoints;
using WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Load configuration (appsettings + environment variables are included by default in .NET)
var configuration = builder.Configuration;

// Add OpenAPI/Swagger via NSwag with app-level metadata (title, description, version) from env variables
var serviceName = Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "Weather Data API";
var serviceVersion = Environment.GetEnvironmentVariable("SERVICE_VERSION") ?? "v1";
var serviceDescription =
    "Backend API that serves mock weather forecast data. Configure defaults using environment variables." +
    " Endpoints are grouped under 'Weather'.";

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = serviceName;
    settings.Version = serviceVersion;
    settings.Description = serviceDescription;
    settings.PostProcess = document =>
    {
        document.Info = new OpenApiInfo
        {
            Title = serviceName,
            Version = serviceVersion,
            Description = serviceDescription
        };
        document.Tags = new[]
        {
            new NSwag.OpenApiTag { Name = "Weather", Description = "Operations related to weather forecast" }
        }.ToList();
    };
});

// Configure strongly-typed options from environment variables
builder.Services.Configure<WeatherOptions>(opts =>
{
    // Defaults
    opts.DefaultUnits = "metric"; // C
    opts.DefaultForecastDays = 5;

    // Environment overrides
    var envUnits = Environment.GetEnvironmentVariable("WEATHER_DEFAULT_UNITS");
    if (!string.IsNullOrWhiteSpace(envUnits))
    {
        opts.DefaultUnits = envUnits.Trim();
    }

    var envDays = Environment.GetEnvironmentVariable("WEATHER_FORECAST_DAYS");
    if (int.TryParse(envDays, out var parsedDays))
    {
        // Bound days for sanity
        opts.DefaultForecastDays = Math.Clamp(parsedDays, 1, 14);
    }
});

// Register application services
builder.Services.AddSingleton<IWeatherService, WeatherService>();

// Add CORS - allow specific origins from env var ALLOWED_ORIGINS (comma separated), fallback to allow all
builder.Services.AddCors(options =>
{
    options.AddPolicy("ConfiguredCors", policy =>
    {
        var allowedOriginsCsv = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS");
        if (!string.IsNullOrWhiteSpace(allowedOriginsCsv))
        {
            var origins = allowedOriginsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (origins.Length > 0)
            {
                policy.WithOrigins(origins)
                      .AllowAnyMethod()
                      .AllowAnyHeader();
                return;
            }
        }

        // Fallback: allow all (development friendly)
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors("ConfiguredCors");

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
    config.DocumentTitle = $"{serviceName} {serviceVersion} - API Docs";
});

 // Health check endpoint (root)
app.MapGet("/", () => Results.Ok(new { message = "Healthy" }))
    .WithName("HealthCheck")
    .WithTags("Health");

// Map Weather endpoints
app.MapWeatherEndpoints();

app.Run();
