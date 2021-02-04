using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using Application.SMS.RENEWAL.Commands;
using Common;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace WorkerTestRenewal
{
    public class WorkerTestRenewal : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WorkerTestRenewal>();
        private readonly IOptions<RabbitMQAuth> _RabbitMQAppSetting;
        public IServiceProvider Services { get; }

        public WorkerTestRenewal(IServiceProvider service, IOptions<RabbitMQAuth> RabbitMQAppSetting)
        {
            Services = service;
            _RabbitMQAppSetting = RabbitMQAppSetting;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                int interval = 1000;
                try
                {
                    _logger.Information("Test Worker RENEWAL running....");

                    var sw = Stopwatch.StartNew();
                    var iQueueAuth = _RabbitMQAppSetting.Value;
                    using (var scope = Services.CreateScope())
                    {
                        var mediator =
                        scope.ServiceProvider
                            .GetRequiredService<IMediator>();
                        //start prep Daily Renewal for services
                        var vm = await mediator.Send(new DailyPrepRenewal
                        {
                            RenewalTime = DateTime.Today,
                            QueueAuth = iQueueAuth
                        }, stoppingToken);
                    }
                    sw.Stop();
                    _logger.Information("Total elapse Time test renewal process : {sw}", sw.Elapsed);
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
