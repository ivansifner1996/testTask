using System;

namespace UpriseTask.Mappings.DTO
{
    public class ProductionResponse
    {
        public DateTimeOffset TimeStamp { get; set; }
        public double ProductionValue { get; set; }
    }
}
