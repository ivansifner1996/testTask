using System;
using System.Collections.Generic;

namespace UpriseTask.Mappings.DTO
{
    public class SolarPlantDto
    {
        public string Name { get; set; }
        public double InstalledPower { get; set; }
        public DateTimeOffset DateOfInstallation { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<SolarPlantProductionDto>? SolarPlantProductions { get; set; }
    }
}
