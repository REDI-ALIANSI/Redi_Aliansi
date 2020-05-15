using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.SMS.SMSOUT.Commands;
using MediatR;
using Microsoft.Extensions.Hosting;
using Serilog;
//using Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Common;

namespace ServiceSMSOUT
{
    public class WorkerSmsout : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WorkerSmsout>();
        private readonly IOptions<RabbitMQAuth> _RabbitMQAppSetting;
        public IServiceProvider Services { get; }

        public WorkerSmsout(IServiceProvider service, IOptions<RabbitMQAuth> RabbitMQAppSetting)
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
                    _logger.Information("Worker SMSOUT running...");
                    var sw = Stopwatch.StartNew();
                    var iQueueAuth = _RabbitMQAppSetting.Value;
                    using (var scope = Services.CreateScope())
                    {
                        var mediator =
                        scope.ServiceProvider
                            .GetRequiredService<IMediator>();

                        var smsoutVm = await mediator.Send(new ProcessSmsoutQueueCommand
                        {
                            Queue = "SMSOUTQ",
                            QueueAuth = iQueueAuth
                        }, stoppingToken);
                        _logger.Information("Processed SMSOUTD Msisdn:{msisdn} Mt_Message:{Mt_Message} MtTxId:{MtTxId} IsDnWatch:{IsDnWatch} ServiceId:{ServiceId} OperatorId:{OperatorId} Shortcode:{Shortcode} Status:{Status}",
                            smsoutVm.Msisdn,
                            smsoutVm.Mt_Message,
                            smsoutVm.MtTxId,
                            smsoutVm.IsDnWatch.ToString(),
                            smsoutVm.ServiceId,
                            smsoutVm.OperatorId,
                            smsoutVm.Shortcode,
                            smsoutVm.Status);
                    }
                    
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
