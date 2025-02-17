using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using UpriseTask.Controllers;
using UpriseTask.Data;
using UpriseTask.Entities;
using UpriseTask.Mappings.DTO;
using UpriseTask.models;
using UpriseTask.Services;
using Xunit;

public class ProductionSummaryControllerTests
{
    private readonly Mock<IWeatherService> _mockWeatherService;
    private readonly SolarPlantContext _context;

    public ProductionSummaryControllerTests()
    {
        _mockWeatherService = new Mock<IWeatherService>();

        var options = new DbContextOptionsBuilder<SolarPlantContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new SolarPlantContext(options);
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        _context.SolarPlants.Add(new SolarPlantEntity { Id = 10, InstalledPower = 100, Longitude = 10.5, Latitude = 20.5 });
        _context.SolarPlantProductions.AddRange(new[]
        {
            new SolarPlantProductionEntity
            {
                Id = 1,
                SolarPlantId = 10,
                Timestamp = new DateTimeOffset(2024, 6, 28, 12, 0, 0, TimeSpan.Zero),
            },
            new SolarPlantProductionEntity
            {
                Id = 2,
                SolarPlantId = 10,
                Timestamp = new DateTimeOffset(2024, 6, 28, 12, 15, 0, TimeSpan.Zero),
            }
        });

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetProductSummary_ReturnsBadRequest_WhenFromDateIsGreaterThanToDate()
    {
        // Arrange
        var controller = new ProductionSummaryController(_context, _mockWeatherService.Object);

        // Act
        var result = await controller.GetProductSummary(1, TimeSeriesType.Forecasted, DateTimeOffset.Now, DateTimeOffset.Now.AddDays(-1), GranularityType.FifteenMinutes);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Timeseries is not valid", badRequestResult.Value);
    }

    [Fact]
    public async Task GetProductSummary_ReturnsNotFound_WhenNoDataExists()
    {
        // Arrange
        var controller = new ProductionSummaryController(_context, _mockWeatherService.Object);

        // Act
        var result = await controller.GetProductSummary(999, TimeSeriesType.Actual, DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now, GranularityType.FifteenMinutes);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        var response = Assert.IsType<NotFoundResponse>(notFoundResult.Value);
        Assert.NotNull(response);
        Assert.Equal("No measured data for specified period", response.Message);
        Assert.Empty(response.Data);
    }

    [Fact]
    public async Task GetProductSummary_ReturnsCorrectProduction_ForMeasuredData()
    {
        // Arrange
        var controller = new ProductionSummaryController(_context, _mockWeatherService.Object);

        //var data = _context.SolarPlantProductions.ToList();


        // Act
        var result = await controller.GetProductSummary(10, TimeSeriesType.Actual,
            new DateTimeOffset(2024, 6, 28, 12, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 6, 28, 13, 0, 0, TimeSpan.Zero),
            GranularityType.FifteenMinutes);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var productions = Assert.IsAssignableFrom<IEnumerable<ProductionResponse>>(okResult.Value);
        Assert.Equal(2, productions.Count());
        Assert.Equal(200, productions.Sum(x => x.ProductionValue));

    }

    [Fact]
    public async Task GetProductSummary_ReturnsForecastedProduction_WithWeatherData()
    {
        // Arrange
        _mockWeatherService.Setup(ws => ws.GetRandomWeatherMetric(20.5, 10.5)).ReturnsAsync(1.5);

        var controller = new ProductionSummaryController(_context, _mockWeatherService.Object);

        // Act
        var result = await controller.GetProductSummary(10, TimeSeriesType.Forecasted,
            new DateTimeOffset(2024, 6, 28, 12, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 6, 28, 13, 0, 0, TimeSpan.Zero),
            GranularityType.OneHour);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var productions = Assert.IsAssignableFrom<IEnumerable<ProductionResponse>>(okResult!.Value);

        Assert.Single(productions);
        Assert.Equal(new DateTimeOffset(2024, 6, 28, 12, 0, 0, TimeSpan.Zero), productions.ElementAt(0).TimeStamp);
        Assert.Equal(300, productions.ElementAt(0).ProductionValue);
    }
}