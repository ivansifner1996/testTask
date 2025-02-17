using System;

namespace UpriseTask.Mappings.DTO
{
    public class SolarPlantProductionDto
    {
        public int Id { get; set; }
        public int SolarPlantId { get; set; }
        public DateTimeOffset Timestamp { get; set; }

    }
}
