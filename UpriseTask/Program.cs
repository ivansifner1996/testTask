using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using UpriseTask.Data;
using UpriseTask.Services.LoggingService;

namespace UpriseTask
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<SolarPlantContext>();
                DataSeed.Seed(context);
            }

            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                      var logPath = hostingContext.Configuration["Logging:LogFilePath"]
                        ?? Path.Combine(hostingContext.HostingEnvironment.ContentRootPath + $"{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}");

                      if (!Directory.Exists(logPath))
                        {
                          Directory.CreateDirectory(logPath);
                        }

                      loggerConfiguration.Enrich.FromLogContext()
                     .Enrich.WithProperty("Application", "UPRISE_TASK")
                     .Enrich.WithProperty("MachineName", Environment.MachineName)
                     .Enrich.WithProperty("CurrentManagedThreadId", Environment.CurrentManagedThreadId)
                     .Enrich.WithProperty("OSVersion", Environment.OSVersion)
                     .Enrich.WithProperty("Version", Environment.Version)
                     .Enrich.WithProperty("UserName", Environment.UserName)
                     .Enrich.WithProperty("ProcessId", Process.GetCurrentProcess().Id)
                     .Enrich.WithProperty("ProcessName", Process.GetCurrentProcess().ProcessName)
                     .WriteTo.File(formatter: new TextFormatter(), path: Path.Combine(logPath, $"uprise_{DateTime.Now:yyyyMMddHH-mmss}.txt"))
                     .ReadFrom.Configuration(hostingContext.Configuration);
                     });

                    webBuilder.UseStartup<Startup>();
                      
                });
    }
}
