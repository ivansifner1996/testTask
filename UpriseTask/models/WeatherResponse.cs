using System.Text.Json.Serialization;

namespace UpriseTask.models
{
    public class WeatherResponse
    {
        [JsonPropertyName("hourly")]
        public HourlyData Hourly { get; set; }
    }

    public class HourlyData
    {
        [JsonPropertyName("cloudcover")]
        public double[] CloudCover { get; set; }
    }
}
