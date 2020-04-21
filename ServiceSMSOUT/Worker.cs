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

namespace ServiceSMSOUT
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<Worker>();
        private readonly IMediator _mediator;

        public Worker(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.Information("Worker SMSOUT running...");
                    var sw = Stopwatch.StartNew();
                    var smsoutVm = await _mediator.Send(new ProcessSmsoutQueueCommand
                    {
                        Queue = "SMSOUTQ"
                    });
                    _logger.Information("Processed SMSOUTD Msisdn:{msisdn} Mt_Message:{Mt_Message} MtTxId:{MtTxId} IsDnWatch:{IsDnWatch} ServiceId:{ServiceId} OperatorId:{OperatorId} Shortcode:{Shortcode} Status:{Status}",
                        smsoutVm.Msisdn,
                        smsoutVm.Mt_Message,
                        smsoutVm.MtTxId,
                        smsoutVm.IsDnWatch.ToString(),
                        smsoutVm.ServiceId,
                        smsoutVm.OperatorId,
                        smsoutVm.Shortcode,
                        smsoutVm.Status);
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
