using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpriseTask.Data;
using UpriseTask.Entities;
using UpriseTask.Mappings.DTO;
using UpriseTask.models;

namespace UpriseTask.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionSummaryController : ControllerBase
    {
        private readonly SolarPlantContext _context;
        private readonly IWeatherService _weatherService;
        public ProductionSummaryController(SolarPlantContext context, IWeatherService weatherService)
        {
            _context = context;
            _weatherService = weatherService;

        }

        [HttpGet("production")]
        public async Task<object> GetProductSummary(
            [FromQuery] int plantId, 
            [FromQuery] TimeSeriesType type,
            [FromQuery] DateTimeOffset from, 
            [FromQuery] DateTimeOffset to,
            [FromQuery] GranularityType granularity)
        {
            if(from >= to)return BadRequest("Timeseries is not valid");

            var production = await _context.SolarPlantProductions
                .Where(prod => prod.SolarPlantId == plantId &&
                               //prod.TimeSeriesType == type &&
                               prod.Timestamp >= from &&
                               prod.Timestamp < to
                ).Include(par => par.SolarPlant)
                .Select(obj => new
                {
                    Longitude = obj.SolarPlant.Longitude,
                    Latitude = obj.SolarPlant.Latitude,
                    InstalledPower = obj.SolarPlant.InstalledPower,
                    Productions = obj
                })
                .ToListAsync();



            if (production.Any())
            {
                //Covering scenario where random weather metric may return result as 0, then we should cast it to null
                double? myRandomMetric = type == TimeSeriesType.Forecasted
                ? await Task.Run(async () =>
                {
                    var fetchedResult = await _weatherService.GetRandomWeatherMetric(production[0].Latitude, production[0].Longitude);
                    return fetchedResult == 0 ? (double?)null : fetchedResult;
                }) : null;

                IEnumerable<ProductionResponse> groupedProductions = granularity switch
                {
                    GranularityType.FifteenMinutes => production.Select(p => new ProductionResponse
                    {
                        TimeStamp = p.Productions.Timestamp,
                        ProductionValue = p.InstalledPower * (myRandomMetric ?? 1) //My random function calculator if forecast is used
                    }),
                    GranularityType.OneHour => production.GroupBy(p => new
                    {
                        Year = p.Productions.Timestamp.Year,
                        Month = p.Productions.Timestamp.Month,
                        Day = p.Productions.Timestamp.Day,
                        Hour = p.Productions.Timestamp.Hour
                    }).Select(g => new ProductionResponse
                    {
                        TimeStamp = new DateTimeOffset(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, 0, 0, TimeSpan.Zero),
                        ProductionValue = g.Sum(x => x.InstalledPower * (myRandomMetric ?? 1))
                    }),
                    _ => throw new ArgumentException("Granularity type is wrong")
                };


                return Ok(groupedProductions.OrderBy(prod => prod.TimeStamp));

            }

            return NotFound(new NotFoundResponse
            {
                Message = "No measured data for specified period",
                Data = Enumerable.Empty<string>()
            });
        }
    }
}