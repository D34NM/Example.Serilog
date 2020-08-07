using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Example.Api.Controllers
{
    [ApiController]
    [Route("weather")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("info")]
        public IActionResult Info()
        {           
            var response = Compute();

            _logger.LogInformation("Computed: {@response}", response);

            return Ok(new { Message = "Info" });
        }

        [HttpGet("warning")]
        public IActionResult Warning()
        {
            var response = Compute();

            _logger.LogWarning("Computed: {@response}", response);
            
            return Ok(new { Message = "Warning" });
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            try
            {
                var response = RaiseException();
            }
            catch (Exception exception)
            {
                _logger.LogError("Error: {@exception}", exception);
            }
            
            return Ok(new { Message = "Error" });
        }

        [HttpGet("critical")]
        public IActionResult Critical()
        {
            try
            {
                var response = RaiseException();
            }
            catch (Exception exception)
            {
                _logger.LogCritical("Critical: {@exception}", exception);
            }
            
            return Ok(new { Message = "Critical" });
        }

        private IEnumerable<WeatherForecast> Compute()
        {
            var rng = new Random();
            var response =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return response;
        }

        private WeatherForecast RaiseException()
        {
            throw new Exception("Something went wrong");
        }
    }
}
