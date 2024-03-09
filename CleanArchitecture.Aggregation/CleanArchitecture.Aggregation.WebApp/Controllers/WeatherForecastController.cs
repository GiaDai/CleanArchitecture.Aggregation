using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CleanArchitecture.Aggregation.WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IHubContext<SignalrHub> _hubContext;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        IHubContext<SignalrHub> hubContext,
        ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        // push message to SignalR hub
        await _hubContext.Clients.All.SendAsync("messageReceived", "WeatherForecastController", "Get method called");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
