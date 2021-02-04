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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Common;

namespace ServiceSMSOUTPUSH
{
    public class WorkerSmsoutPush : BackgroundService
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WorkerSmsoutPush>();
        private readonly IOptions<RabbitMQAuth> _RabbitMQAppSetting;
        private readonly IOptions<WorkerConfig> _workerConfig;
        public IServiceProvider Services { get; }

        public WorkerSmsoutPush(IServiceProvider service, 
            IOptions<RabbitMQAuth> RabbitMQAppSetting,
            IOptions<WorkerConfig> workerConfig)
        {
            Services = service;
            _RabbitMQAppSetting = RabbitMQAppSetting;
            _workerConfig = workerConfig;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var noOfWorkers = _workerConfig.Value.NoOfParallelWorker;
                _logger.Information("Start {noOfWorkers} Tasks", noOfWorkers.ToString());
                var workers_push = new List<Task>();

                for (int i = 1; i <= noOfWorkers; i++)
                {
                    workers_push.Add(SmsoutPushWork(i,stoppingToken));
                }

                await Task.WhenAll(workers_push.ToArray());
            }
        }

        private async Task SmsoutPushWork(int taskN,CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.Information("Task Worker SMSOUTPUSH no. {no} running...",taskN.ToString());
                    var sw = Stopwatch.StartNew();
                    var iQueueAuth = _RabbitMQAppSetting.Value;
                    int Delay = 1000;
                    using (var scope = Services.CreateScope())
                    {
                        var mediator =
                        scope.ServiceProvider
                            .GetRequiredService<IMediator>();

                        var smsoutVm = await mediator.Send(new ProcessSmsoutQueueCommand
                        {
                            Queue = "SMSOUTP",
                            QueueAuth = iQueueAuth
                        }, stoppingToken);
                        if (smsoutVm.Status.Equals(200))
                        {
                            _logger.Information("Processed SMSOUTP Msisdn:{msisdn} Mt_Message:{Mt_Message} MtTxId:{MtTxId} IsDnWatch:{IsDnWatch} ServiceId:{ServiceId} OperatorId:{OperatorId} Sid:{Sid} Status:{Status} URL Hit:{url} TaskNo: {taskN}",
                            smsoutVm.Msisdn,
                            smsoutVm.Mt_Message,
                            smsoutVm.MtTxId,
                            smsoutVm.IsDnWatch.ToString(),
                            smsoutVm.ServiceId,
                            smsoutVm.OperatorId,
                            smsoutVm.Sid,
                            smsoutVm.Status,
                            smsoutVm.URI_Hit,
                            taskN.ToString());
                        }
                        else if (smsoutVm.Status.Equals(500))
                        {
                            Delay = 60000;
                            _logger.Information("Status: {status} Trx_Status: {trx_status} URL Hit:{url} TaskNo: {taskN}", smsoutVm.Status, smsoutVm.Trx_Status, smsoutVm.URI_Hit, taskN.ToString());
                        }
                        else
                            _logger.Information("Status: {status} Trx_Status: {trx_status} URL Hit:{url} TaskNo: {taskN}", smsoutVm.Status, smsoutVm.Trx_Status, smsoutVm.URI_Hit, taskN.ToString());
                    }
                    sw.Stop();
                    _logger.Information("Worker SMSOUTPUSH TaskNo: {taskN} Done in elapse time : {time}", taskN.ToString(), sw.Elapsed.TotalMilliseconds);
                    await Task.Delay(Delay, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.Error("Worker task No. {taskN} Error : {error}",taskN.ToString(),ex.ToString());
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // DO YOUR STUFF HERE
            _logger.Information("Worker is shutdown");
            await base.StopAsync(cancellationToken);
        }
    }
}
