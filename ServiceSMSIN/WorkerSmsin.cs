using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.SMS.SMSIN.Commands;
using MediatR;
using Microsoft.Extensions.Hosting;
using Serilog;
//using Application;
using Microsoft.Extensions.DependencyInjection;
using Common;
using Microsoft.Extensions.Options;

namespace ServiceSMSIN
{
    public class WorkerSmsin : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WorkerSmsin>();
        private readonly IOptions<RabbitMQAuth> _RabbitMQAppSetting;
        public IServiceProvider Services { get; }

        public WorkerSmsin(IServiceProvider service, IOptions<RabbitMQAuth> RabbitMQAppSetting)
        {
            Services = service;
            _RabbitMQAppSetting = RabbitMQAppSetting;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.Information("Worker SMSIN running...");
                    var sw = Stopwatch.StartNew();
                    using (var scope = Services.CreateScope())
                    {
                        var mediator =
                        scope.ServiceProvider
                            .GetRequiredService<IMediator>();

                        var smsinVM = await mediator.Send(new ProcessSmsinQueueCommand
                        {
                            queue = "SMSINQ",
                            appsDllPath = @"D:\services\Dll\",
                            QueueAuth = _RabbitMQAppSetting.Value
                        }, stoppingToken);
                        _logger.Information("Processed SMSIN Msisdn:{msisdn} Mo_Message:{mo_message} MotxId:{motxid} ServiceId:{serviceid} Shortcode:{shortcode} Status:{status}",
                            smsinVM.Msisdn,
                            smsinVM.Mo_Message,
                            smsinVM.MotxId,
                            smsinVM.ServiceId,
                            smsinVM.Shortcode,
                            smsinVM.Status);
                    }
                    sw.Stop();
                    _logger.Information("Worker SMSIN Done in elapse time : {time}", sw.Elapsed.TotalMilliseconds);
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.Error("Worker Error : " + ex.ToString());
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
