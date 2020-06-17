using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.SMS.RENEWAL.Commands;
using Common;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace ServiceRENEWAL
{
    public class WorkerRenewal : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WorkerRenewal>();
        private readonly IOptions<RabbitMQAuth> _RabbitMQAppSetting;
        //private readonly IMediator _mediator;

        /*public Worker(IMediator mediator)
        {
            _mediator = mediator;
        }*/
        public IServiceProvider Services { get; }

        public WorkerRenewal(IServiceProvider service, IOptions<RabbitMQAuth> RabbitMQAppSetting)
        {
            Services = service;
            _RabbitMQAppSetting = RabbitMQAppSetting;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                /*
                TimeSpan interval = TimeSpan.FromHours(24);
                //calculate time to run the first time & delay to set the timer
                //DateTime.Today gives time of midnight 00.00
                var nextRunTime = DateTime.Today.AddDays(1).AddHours(1);
                var curTime = DateTime.Now;
                var firstInterval = nextRunTime.Subtract(curTime);

                //Check if its 1 AM
                if (DateTime.Now.Equals(DateTime.Today.AddHours(1)))
                {
                    try
                    {
                        _logger.Information("Worker RENEWAL running....");

                        var sw = Stopwatch.StartNew();
                        using (var scope = Services.CreateScope())
                        {
                            var mediator =
                            scope.ServiceProvider
                                .GetRequiredService<IMediator>();
                            //start prep Daily Renewal for services
                            await mediator.Send(new DailyPrepRenewal
                            {
                                RenewalTime = DateTime.Today
                            }, stoppingToken);
                        }

                        await Task.Delay(interval, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Worker Error : " + ex.ToString());
                        await Task.Delay(interval, stoppingToken);
                    }
                }
                //If Not 1 AM Get new interval for next day 1 AM
                else await Task.Delay(firstInterval, stoppingToken);
            }*/
                TimeSpan interval = TimeSpan.FromHours(24);
                try
                {
                    _logger.Information("Worker RENEWAL running....");

                    var sw = Stopwatch.StartNew();
                    var iQueueAuth = _RabbitMQAppSetting.Value;
                    using (var scope = Services.CreateScope())
                    {
                        var mediator =
                        scope.ServiceProvider
                            .GetRequiredService<IMediator>();
                        //start prep Daily Renewal for services
                        await mediator.Send(new DailyPrepRenewal
                        {
                            RenewalTime = DateTime.Today,
                            QueueAuth = iQueueAuth
                        }, stoppingToken);
                    }

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
