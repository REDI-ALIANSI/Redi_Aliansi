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

namespace ServiceRENEWAL
{
    public class WorkerRenewal : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WorkerRenewal>();
        private readonly IOptions<RabbitMQAuth> _RabbitMQAppSetting;
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
                TimeSpan interval = TimeSpan.FromHours(24);
                //calculate time to run the first time & delay to set the timer
                //DateTime.Today gives time of midnight 00.00
                var nextRunTime = DateTime.Today.AddDays(1).AddHours(6);
                var curTime = DateTime.Now;
                var firstInterval = nextRunTime.Subtract(curTime);

                //Check if its 6 AM
                if (DateTime.Now >= DateTime.Today.AddHours(6) && DateTime.Now < DateTime.Today.AddHours(7))
                {
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
                            var renewVMs = await mediator.Send(new DailyPrepRenewal
                            {
                                RenewalTime = DateTime.Today,
                                QueueAuth = iQueueAuth
                            }, stoppingToken);

                            foreach (var renewVM in renewVMs)
                            {
                                _logger.Information("Renewal service {service} messages have been generated count: {messageCount}, Time Elapse : {elapse}",
                                    renewVM.ServiceName,
                                    renewVM.MessagesGenerated,
                                    renewVM.GenerateSpan);
                            }
                        }
                        sw.Stop();
                        _logger.Information("Total elapse Time renewal process : {sw}", sw.Elapsed);
                        _logger.Information("Next renewal in: {interval}", interval);
                        await Task.Delay(interval, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Worker Error : " + ex.ToString());
                        await Task.Delay(interval, stoppingToken);
                    }
                }
                //If Not 1 AM Get new interval for next day 1 AM
                else
                {
                    _logger.Information("Its not 6 AM!");
                    _logger.Information("Next renewal in: {interval}", firstInterval);
                    await Task.Delay(firstInterval, stoppingToken);
                }
            }
        }
    }
}
