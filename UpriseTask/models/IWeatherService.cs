using System.Threading.Tasks;

namespace UpriseTask.models
{
    public interface IWeatherService
    {
        Task<double> GetRandomWeatherMetric(double latitude, double longitude);
    }
}
