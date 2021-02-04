using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Application;
using Presistence;
using Microsoft.Extensions.Configuration;
using Infrastructure;
using WorkerTestRenewal.Services;
using Application.Common.Interfaces;
using Common;

namespace WorkerTestRenewal
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"D:\services\log\test\renewal\renewal-general-log-.log",
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
                    services.AddHostedService<WorkerTestRenewal>();
                });
    }
}
