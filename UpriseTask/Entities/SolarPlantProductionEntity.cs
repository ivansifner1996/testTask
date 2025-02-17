using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpriseTask.Entities
{
    public enum TimeSeriesType : short
    {
        [Description("A")]
        Actual = 1,

        [Description("F")]
        Forecasted = 2,
    }


    public enum GranularityType : short
    {

        [Description("15m")]
        FifteenMinutes = 1,

        [Description("1hr")]
        OneHour = 2


    }

    [Table("SolarPlantProduction")]
    public class SolarPlantProductionEntity: BaseEntity<int>
    {
        [Required]
        public int SolarPlantId { get; set; }

        [Required]
        public DateTimeOffset Timestamp { get; set; }

        public SolarPlantEntity SolarPlant { get; set; }


    }
}
