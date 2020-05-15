using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.SMS.RENEWAL.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ServiceRENEWAL
{
    public class WorkerRenewal : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WorkerRenewal>();
        //private readonly IMediator _mediator;

        /*public Worker(IMediator mediator)
        {
            _mediator = mediator;
        }*/
        public IServiceProvider Services { get; }
        public WorkerRenewal(IServiceProvider service)
        {
            Services = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
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

                        await mediator.Send(new DailyPrepRenewal
                        {
                            RenewalTime = DateTime.Today
                        }, stoppingToken);
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
