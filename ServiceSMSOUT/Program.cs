using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Application;
using Microsoft.Extensions.Configuration;
using Presistence;
using Application.Common.Interfaces;
using Infrastructure;
using Common;
using ServiceSMSOUT.Services;
using Serilog.Events;

namespace ServiceSMSOUT
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"D:\services\log\SMS\smsout\worker\smsout-general-log-.log",
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
                    services.Configure<RabbitMQAuth>(configuration.GetSection("RabbitMQAuth"));
                    services.Configure<WorkerConfig>(configuration.GetSection("WorkerConfig"));
                    services.AddScoped<ICurrentUserService, CurrentUserService>();

                    services.AddHostedService<WorkerSmsout>();
                })
                .UseSerilog();
    }
}
