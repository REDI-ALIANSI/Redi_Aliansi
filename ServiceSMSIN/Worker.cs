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

namespace ServiceSMSIN
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

                    _logger.Information("Worker SMSIN running...");
                    var sw = Stopwatch.StartNew();
                    var smsinVM = await _mediator.Send(new ProcessSmsinQueueCommand
                    {
                        queue = "SMSINQ",
                        appsDllPath = @"D:\services\Dll\"
                    });
                    _logger.Information("Processed SMSIN Msisdn:{msisdn} Mo_Message:{mo_message} MotxId:{motxid} ServiceId:{serviceid} Shortcode:{shortcode} Status:{status}", 
                        smsinVM.Msisdn,
                        smsinVM.Mo_Message,
                        smsinVM.MotxId,
                        smsinVM.ServiceId,
                        smsinVM.Shortcode,
                        smsinVM.Status);
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
