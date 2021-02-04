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

namespace WorkerGenerateDailyReports
{
    public class WorkerGenerateDailyReports : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WorkerGenerateDailyReports>();
        private readonly IConfiguration _configuration;
        public IServiceProvider Services { get; }

        public WorkerGenerateDailyReports(IServiceProvider service, IConfiguration configuration)
        {
            Services = service;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan interval = TimeSpan.FromHours(24);
                //calculate time to run the first time & delay to set the timer
                //DateTime.Today gives time of midnight 00.00
                var nextRunTime = DateTime.Today.AddDays(1);
                var curTime = DateTime.Now;
                var firstInterval = nextRunTime.Subtract(curTime);

                if (DateTime.Now > DateTime.Today && DateTime.Now < DateTime.Today.AddMinutes(10))
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
                            else _logger.Error("Error Generate revenue report: " + GenerateRevenueRep.Errors.FirstOrDefault());

                            //Start Generate Subscriptions Reports
                            var GenerateSubsReport = await mediator.Send(new GenerateSubscriptionsReports
                                                            { GenDate = Gendate }, stoppingToken);

                            if (GenerateSubsReport.Succeeded) _logger.Information("Generate Subscription Report is done!");
                            else _logger.Error("Error Generate subs report: " + GenerateSubsReport.Errors.FirstOrDefault());

                            //Start Generate Campaign Reports
                            var GenerateCampaignRep = await mediator.Send(new GenerateCampaignReports
                                                            { GenDate = Gendate }, stoppingToken);

                            if (GenerateCampaignRep.Succeeded) _logger.Information("Generate Campaign Report is done!");
                            else _logger.Error("Error Generate Campaign report: " + GenerateCampaignRep.Errors.FirstOrDefault());

                            //Update Generate Report Status to true
                            var UpdateGenReportStatus = await mediator.Send(new UpdateGenReportStatus
                                                                { StatusUpdate = true }, stoppingToken);
                            
                            if (UpdateGenReportStatus.Succeeded) _logger.Information("Generate Report Generation status change to True!");
                            else _logger.Error("Error Update Generate Report Status: " + UpdateGenReportStatus.Errors.FirstOrDefault());
                        }
                        sw.Stop();
                        _logger.Information("Process is done! elapse Time Generate Report process : {sw}", sw.Elapsed);
                        _logger.Information("Next Generate Report in: {interval}", interval);
                        await Task.Delay(interval, stoppingToken);
                    }
                    catch(Exception ex)
                    {
                        _logger.Error("Worker Error : " + ex.ToString());
                        await Task.Delay(interval, stoppingToken);
                    }
                }
                else
                {
                    _logger.Information("Its not 12 AM!");
                    _logger.Information("Next Generate Report in: {interval}", firstInterval);
                    await Task.Delay(firstInterval, stoppingToken);
                }
            }
        }
    }
}
