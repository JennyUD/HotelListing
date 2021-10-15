using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing
{
    public class Program
    {
        public static void Main(string[] args)
        { //added
            Log.Logger = new LoggerConfiguration() 
                .WriteTo.File(
                path: "c:\\hotellistings\\logs\\log-.txt",
                outputTemplate: "(Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz) [{level:u3}] {message:1j} {NewLine} {Exception} ",
              rollingInterval: RollingInterval.Day,
               restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
               ).CreateLogger();
            //added ends
            try
            {
                Log.Information("Application Is Starting");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Applicaion faile to start");
            }
            finally
            {
                Log.CloseAndFlush();
            }



            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog() //added
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
