using Microsoft.AspNetCore.Mvc;
using OData.Neo.Core.Services.Coordinations.OQueries;

namespace OData.Neo.Core.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OQueryController : ControllerBase
    {
        private readonly IOQueryCoordinationService oQueryCoordinationService;

        public OQueryController(IOQueryCoordinationService oQueryCoordinationService)
        {
            this.oQueryCoordinationService = oQueryCoordinationService;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpPost]
        public async Task Test()
        {
            var foo = await oQueryCoordinationService.ProcessOQueryAsync<WeatherForecast>("$select=TemperatureC");

            var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        }

    }
}
