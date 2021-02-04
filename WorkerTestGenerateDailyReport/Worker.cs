using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.SMS.REPORTS.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace WorkerTestGenerateDailyReport
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<Worker>();
        private readonly IConfiguration _configuration;
        public IServiceProvider Services { get; }

        public Worker(IServiceProvider service, IConfiguration configuration)
        {
            Services = service;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.Information("Worker Generate Daily Report running....");
                    var sw = Stopwatch.StartNew();
                    var Gendate = DateTime.Today.AddDays(-1);

                    using (var scope = Services.CreateScope())
                    {
                        var mediator =
                        scope.ServiceProvider
                            .GetRequiredService<IMediator>();

                        //Start Generate Revenue Reports
                        var GenerateRevenueRep = await mediator.Send(new GenerateRevenueReports
                        { GenDate = Gendate }, stoppingToken);

                        if (GenerateRevenueRep.Succeeded) _logger.Information("Generate Revenue Report is done!");

                        //Start Generate Subscriptions Reports
                        var GenerateSubsReport = await mediator.Send(new GenerateSubscriptionsReports
                        { GenDate = Gendate }, stoppingToken);

                        if (GenerateSubsReport.Succeeded) _logger.Information("Generate Subscription Report is done!");

                        //Start Generate Campaign Reports
                        var GenerateCampaignRep = await mediator.Send(new GenerateCampaignReports
                        { GenDate = Gendate }, stoppingToken);

                        if (GenerateCampaignRep.Succeeded) _logger.Information("Generate Campaign Report is done!");

                        //Update Generate Report Status to true
                        var UpdateGenReportStatus = await mediator.Send(new UpdateGenReportStatus
                        { StatusUpdate = true }, stoppingToken);

                        if (UpdateGenReportStatus.Succeeded) _logger.Information("Generate Report Generation status change to True!");
                    }
                    sw.Stop();
                    _logger.Information("Process is done! elapse Time renewal process : {sw}", sw.Elapsed);
                    _logger.Information("Next renewal in: 1000 ms");
                    await Task.Delay(100000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.Error("Worker Error : " + ex.ToString());
                    await Task.Delay(100000, stoppingToken);
                }
            }
        }
    }
}
