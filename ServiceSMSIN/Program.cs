using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.AspNetCore;
using Serilog.Sinks.File;
using Serilog.Filters;
using Application;
using Presistence;
using Application.Common.Interfaces;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Common;
using ServiceSMSIN.Services;

namespace ServiceSMSIN
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"D:\services\log\SMS\smsin\worker\smsin-general-log-.log", 
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] TraceId: {TraceId} Context: {SourceContext} {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            try
            {
                Log.Information("Starting worker");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApplication();

                    IConfiguration configuration = hostContext.Configuration;

                    services.AddPersistence(configuration);
                    services.AddInfrastructure(configuration);
                    services.AddScoped<ICurrentUserService, CurrentUserService>();
                    services.Configure<RabbitMQAuth>(configuration.GetSection("RabbitMQAuth"));

                    services.AddHostedService<WorkerSmsin>();
                })
                .UseSerilog();
    }
}
