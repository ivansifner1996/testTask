using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using UpriseTask.models;

namespace UpriseTask.Services
{
    public class WeatherService: IWeatherService
    {
        private readonly HttpClient _httpClient;
        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double> GetRandomWeatherMetric(double latitude, double longitude)
        {
            try
            {
                string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=cloudcover";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return 1.0;
                }

                var weatherData = await JsonSerializer.DeserializeAsync<WeatherResponse>(
                    await response.Content.ReadAsStreamAsync());

                double cloudCover = weatherData?.Hourly?.CloudCover?[0] ?? 0;

                return cloudCover;


            }
            catch (Exception ex)
            {
              Log.Error(ex, "An error has occured while fetchin Weather random data");
              throw new InvalidOperationException("Error has occured", ex);

            }
        }
    }
}
