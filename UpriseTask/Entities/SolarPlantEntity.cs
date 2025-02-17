using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpriseTask.Entities

{

    [Table("SolarPlant")]
    public class SolarPlantEntity: BaseEntity<int>
    {
        public string? Name { get; set; }

        [Required]
        public double InstalledPower { get; set; }

        [Required]
        public DateTimeOffset DateOfInstallation { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public List<SolarPlantProductionEntity> SolarPlantProductions { get; set; }

    }
}
