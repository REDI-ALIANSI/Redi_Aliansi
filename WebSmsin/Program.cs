using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Presistence;
using Serilog;
using Serilog.Events;
using Serilog.AspNetCore;
using Serilog.Sinks;
using WebSmsin.Controllers;
using Serilog.Filters;

namespace WebSmsin
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var isControllerIndosat = Matching.FromSource("WebSmsin.Controllers.IndosatController");
            var isControllerExcel = Matching.FromSource("WebSmsin.Controllers.ExcelController");
            var isControllerTsel = Matching.FromSource("WebSmsin.Controllers.TselController");

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //.WriteTo.Console(outputTemplate: @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] Context: {SourceContext} TraceId: {TraceId} {Message:lj}{NewLine}{Exception}")
            .Enrich.FromLogContext()
            .WriteTo.Logger( g => g
                .Filter.ByExcluding( i => isControllerIndosat(i) || isControllerExcel(i) || isControllerTsel(i))
                .WriteTo.File(@"D:\services\log\SMS\smsin\API\api-general-log-.log",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] TraceId: {TraceId} Context: {SourceContext} {Message:lj}{NewLine}{Exception}",
                //outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Properties} {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                shared: true))
            .WriteTo.Logger(i => i
                .Filter.ByIncludingOnly(isControllerIndosat)
                .WriteTo.File(@"D:\services\log\SMS\smsin\INDOSAT\indosat-log-.log",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] TraceId: {TraceId} Context: {SourceContext} {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    shared: true))
            .WriteTo.Logger(i => i
                .Filter.ByIncludingOnly(isControllerExcel)
                .WriteTo.File(@"D:\services\log\SMS\smsin\XL\excel-log-.log",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] Context: {SourceContext} {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    shared: true))
            .WriteTo.Logger(i => i
                .Filter.ByIncludingOnly(isControllerTsel)
                .WriteTo.File(@"D:\services\log\SMS\smsin\XL\tsel-log-.log",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] Context: {SourceContext} {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    shared: true))
            .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                var host = CreateWebHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var rediSmsContext = services.GetRequiredService<RediSmsDbContext>();
                    rediSmsContext.Database.Migrate();

                    //var identityContext = services.GetRequiredService<ApplicationDbContext>();
                    //identityContext.Database.Migrate();

                    //var mediator = services.GetRequiredService<IMediator>();
                    //await mediator.Send(new SeedSampleDataCommand(), CancellationToken.None);
                }
                host.Run();
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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true);

                    if (env.IsDevelopment())
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            config.AddUserSecrets(appAssembly, optional: true);
                        }
                    }

                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
