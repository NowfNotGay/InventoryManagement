using Context.Example;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private DB_Testing_Context _Testing_Context;

        public WeatherForecastController(DB_Testing_Context testing_Context)
        {
            _Testing_Context = testing_Context;
        }


        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            var abc = _Testing_Context.MessageContents.ToList();
            int i = 0;
            return Ok(abc);
        } 
    }
}
