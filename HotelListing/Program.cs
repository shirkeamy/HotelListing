using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(
                    path: "ConfigFiles\\ApplicationLogs\\log-.txt",
                    outputTemplate: "[{Timestamp:yyyy MMM dd HH:mm:ss} {Level:u3}] - {Message} : {NewLine}{Exception}",
                    restrictedToMinimumLevel:LogEventLevel.Information,
                    rollingInterval:RollingInterval.Day
                ).CreateLogger();

            try
            {
                Log.Information("Application is started");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error to start an Application");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
