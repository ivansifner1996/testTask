using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using UpriseTask.Entities;

namespace UpriseTask.Data
{
    public static class DataSeed
    {
        public static async void Seed(SolarPlantContext context)
        {
            context.Database.EnsureCreated();

            if (context.SolarPlants.Any()) return;

            DateTimeOffset startDate = DateTimeOffset.Now;

            var solarPlants = new List<SolarPlantEntity>
            {
                new SolarPlantEntity
                {
                    Name = "Solar Plant 1 (Zagreb)",
                    InstalledPower = 1000,
                    DateOfInstallation = DateTimeOffset.Now.AddYears(-3),
                    Latitude = 45.8156,
                    Longitude = 15.98,
                    SolarPlantProductions = GenerateRandomProductionData(1,
                    startDate.AddYears(-1)
                    .AddMinutes(-startDate.Minute)
                    .AddSeconds(-startDate.Second)
                    .AddMilliseconds(-startDate.Millisecond),
                    DateTimeOffset.Now)
                },
                new SolarPlantEntity
                {
                    Name = "Solar Plant 2 (Zadar)",
                    InstalledPower = 2000,
                    DateOfInstallation = DateTimeOffset.Now.AddYears(-2),
                    Latitude = 44.1171,
                    Longitude = 15.2361,
                    SolarPlantProductions = GenerateRandomProductionData(2,
                    startDate.AddYears(-1)
                    .AddMinutes(-startDate.Minute)
                    .AddSeconds(-startDate.Second)
                    .AddMilliseconds(-startDate.Millisecond),
                     DateTimeOffset.Now)
                },
                new SolarPlantEntity
                {
                    Name = "Solar Plant 3 (Osijek)",
                    InstalledPower = 3000,
                    DateOfInstallation = DateTimeOffset.Now.AddYears(-1),
                    Latitude = 45.54928,
                    Longitude = 18.6778,
                    SolarPlantProductions = GenerateRandomProductionData(3, 
                    startDate.AddYears(-1)
                    .AddMinutes(-startDate.Minute)
                    .AddSeconds(-startDate.Second)
                    .AddMilliseconds(-startDate.Millisecond),
                     DateTimeOffset.Now)
                }
            };


            await context.SolarPlants.AddRangeAsync(solarPlants);
            await context.SaveChangesAsync();

        }

        private static List<SolarPlantProductionEntity> GenerateRandomProductionData(int solarPlantId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var productionData = new List<SolarPlantProductionEntity>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dailyIntervals = (24*60) / 15;

                for (int i = 0; i < dailyIntervals; i++)
                {
                    productionData.Add(new SolarPlantProductionEntity
                    {
                        SolarPlantId = solarPlantId,
                        Timestamp = date.AddMinutes(i * 15),

                    });
                }
            }
            return productionData;
        }
    }
}
