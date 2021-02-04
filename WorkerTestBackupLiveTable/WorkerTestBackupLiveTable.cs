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

namespace WorkerTestBackupLiveTable
{
    public class WorkerTestBackupLiveTable : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WorkerTestBackupLiveTable>();
        private readonly IConfiguration _configuration;
        public IServiceProvider Services { get; }

        public WorkerTestBackupLiveTable(IServiceProvider service, IConfiguration configuration)
        {
            Services = service;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan interval = TimeSpan.FromMinutes(5);
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

                            var UpdateGenReportStatus = await mediator.Send(new UpdateGenReportStatus
                            { StatusUpdate = false }, stoppingToken);
                            if (UpdateGenReportStatus.Succeeded)
                            {
                                _logger.Information("Generate Report Generation status change to false!");
                            }
                        }
                        else
                        {
                            interval = TimeSpan.FromMinutes(10);
                            _logger.Information("Backup procedures Delayed for 10 Minutes due to Report Generation not done yet!");
                        }
                    }
                    sw.Stop();
                    _logger.Information("Process is done! elapse Time renewal process : {sw}", sw.Elapsed);
                    _logger.Information("Next renewal in: {interval}", interval);
                    await Task.Delay(interval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.Error("Worker Error : " + ex.ToString());
                    await Task.Delay(interval, stoppingToken);
                }
            }
        }
    }
}
