using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Application.SMS.BACKUP_PROCEDURES.Command;
using MediatR;
using Application.SMS.REPORTS.Queries;
using Application.SMS.REPORTS.Commands;

namespace WorkerBackupLiveTable
{
    public class WorkerBackupLiveTable : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WorkerBackupLiveTable>();
        private readonly IConfiguration _configuration;
        public IServiceProvider Services { get; }

        public WorkerBackupLiveTable(IServiceProvider service, IConfiguration configuration)
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
                var nextRunTime = DateTime.Today.AddDays(1).AddHours(1);
                var curTime = DateTime.Now;
                var firstInterval = nextRunTime.Subtract(curTime);

                if (DateTime.Now > DateTime.Today.AddHours(1) && DateTime.Now < DateTime.Today.AddHours(2))
                {
                    try
                    {
                        _logger.Information("Worker Backup Live Table running....");
                        var sw = Stopwatch.StartNew();
                        
                        using (var scope = Services.CreateScope())
                        {
                            var mediator =
                            scope.ServiceProvider
                                .GetRequiredService<IMediator>();

                            //Check Genrate Report Status
                            var GenReportStatus = await mediator.Send(new CheckGenReportsStatus { }, stoppingToken);
                            if (GenReportStatus)
                            {
                                //start prep Daily Backup live table for services
                                var result = await mediator.Send(new BackupLiveTables
                                {
                                    ProcedureSmsout = "backup_smsoutd",
                                    ProcedureSmsin = "backup_smsind",
                                    Conn = _configuration.GetConnectionString("RediAliansi")
                                }, stoppingToken);

                                if (result.Succeeded)
                                {
                                    _logger.Information("Procedure SMSOUTD: backup_smsoutd done");
                                    _logger.Information("Procedure SMSIND: backup_smsind done");
                                }
                                else _logger.Error("Error backup Procedure(s): " + result.Errors.FirstOrDefault());

                                var UpdateGenReportStatus = await mediator.Send(new UpdateGenReportStatus 
                                                                    { StatusUpdate = false }
                                                                    , stoppingToken);
                                if (UpdateGenReportStatus.Succeeded)
                                {
                                    _logger.Information("Generate Report Generation status change to false!");
                                }
                                else _logger.Error("Error Update Generate Report Status: " + UpdateGenReportStatus.Errors.FirstOrDefault());
                            }
                            else
                            {
                                interval = TimeSpan.FromMinutes(10);
                                _logger.Error("Backup procedures Delayed for 10 Minutes due to Report Generation not done yet!");
                            }
                        }
                        sw.Stop();
                        _logger.Information("Process is done! elapse Time Backup Live Table process : {sw}", sw.Elapsed);
                        _logger.Information("Next Time Backup Live Table in: {interval}", interval);
                        await Task.Delay(interval, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Worker Error : " + ex.ToString());
                        await Task.Delay(interval, stoppingToken);
                    }
                }
                else
                {
                    _logger.Information("Its not 1 AM!");
                    _logger.Information("Next Generate Report in: {interval}", firstInterval);
                    await Task.Delay(firstInterval, stoppingToken);
                }
            }
        }
    }
}
