using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Application.Common.Exceptions;
using Application.SMS.SUBSCRIPTION.Queries;
using Application.SMS.SMSOUT.Commands;
using Application.SMS.MESSAGE.Command;
using Application.Common.Behaviour;
using Application.SMS.SUBSCRIPTION.Commands;
using Application.SMS.RENEWAL.ViewModel;
using System.Diagnostics;
using Application.SMS.SERVICE.Queries;
using Application.SMS.SERVICE.ViewModel;
using System.Text.Json;

namespace Application.SMS.RENEWAL.Commands
{
    public class ProcessRenewalConfigHandler : IRequestHandler<ProcessRenewalConfig,RenewalVM>
    {
        private readonly IMediator _mediator;
        private readonly IRediSmsDbContext _context;
        private readonly IHttpRequest _httpRequest;

        public ProcessRenewalConfigHandler(IMediator mediator, IRediSmsDbContext context, IHttpRequest httpRequest)
        {
            _mediator = mediator;
            _context = context;
            _httpRequest = httpRequest;
        }

        public async Task<RenewalVM> Handle(ProcessRenewalConfig request, CancellationToken cancellationToken)
        {
            try
            {
                DateTime NextRenewalDate = new DateTime();
                var renewVM = new RenewalVM();
                var sw = Stopwatch.StartNew();
                int MessageCount = 0;
                var listSubscription = await _mediator.Send(new GetSubscriptionByServiceOperator
                {
                    ServiceId = request.renewalConfig.ServiceId,
                    OperatorId = request.renewalConfig.OperatorId,
                    rRenewalDate = request.rRenewalTime
                }, cancellationToken);

                if (listSubscription.Count > 0)
                {
                    var today = DateTime.Today.DayOfWeek;
                    //GET Renewal Message from content
                    var rMessage = await _context.Messages.AsNoTracking().Where(m => m.MessageId == request.renewalConfig.MessageId).FirstOrDefaultAsync();
                    
                    var rTxtMessage = await _mediator.Send(new GetRenewalMessage { rMessage = rMessage, rRenewalDate = DateTime.Today },cancellationToken);
                    
                    //If renewal message is empty send Email warning notification here
                    if (!String.IsNullOrEmpty(rTxtMessage))
                    {
                        rMessage.MessageTxt = rTxtMessage;
                    }

                    if (!request.renewalConfig.IsSequence && request.renewalConfig.ScheduleDay == today)
                    {
                        //Next renewal order
                        int RenewalConfigNextOrder = request.renewalConfig.ScheduleOrder.Equals(request.rRenewalConfigCount) ?
                                                    1 : request.renewalConfig.ScheduleOrder + 1;

                        var NextRenewalDay = await _context.ServiceRenewalConfigurations
                                                .Where(c => c.ServiceId.Equals(request.renewalConfig.ServiceId)
                                                && c.OperatorId.Equals(request.renewalConfig.OperatorId)
                                                && c.IsActive
                                                && c.ScheduleOrder.Equals(RenewalConfigNextOrder))
                                                .Select(r => r.ScheduleDay)
                                                .FirstOrDefaultAsync();

                        NextRenewalDate = await _mediator.Send(new GetNextDayofWeekDate { DayofWeek = NextRenewalDay },cancellationToken);

                        var service = await _mediator.Send(new GetServiceById { ServiceId = request.renewalConfig.ServiceId }, cancellationToken);

                        //send message to SMSOUTP Queue
                        foreach (var Subscription in listSubscription)
                        {
                            // Make function for put renewal message to SMSOUTP Queue
                            if (request.renewalConfig.ActiveDll)
                            {
                                string Uri = service.ServiceCustom = "/Renewal";
                                var ReqObj = new CustomServiceRenewalRequest() { subscription = Subscription, message = rMessage.MessageTxt };
                                var Resp = await _httpRequest.PostHttpResp(Uri, ReqObj);
                                var RespCustApi = JsonSerializer.Deserialize<CustomServiceRenewalResponse>(Resp);
                                if (RespCustApi.result.Succeeded)
                                {
                                    if (!String.IsNullOrEmpty(RespCustApi.cMessage))
                                    {
                                        rMessage.MessageTxt = RespCustApi.cMessage;
                                    }
                                }
                            }

                            if (!String.IsNullOrEmpty(rMessage.MessageTxt))
                            {
                                string iQueue = "SMSOUTP";
                                var MtTxId = Guid.NewGuid().ToString("N");
                                await _mediator.Send(new SendSmsoutQueueCommand
                                {
                                    rMessage = rMessage,
                                    rMsisdn = Subscription.Msisdn,
                                    rSparam = String.Empty,
                                    rIparam = 0,
                                    rQueue = iQueue,
                                    rServiceId = Subscription.ServiceId,
                                    rMtTxId = MtTxId,
                                    QueueAuth = request.QueueAuth
                                });
                                MessageCount ++;
                            }
                            //Update Subs for next Renewal Date
                            await _mediator.Send(new UpdateSubscriptionRenewal { subscription = Subscription, rNextRenewalDate = NextRenewalDate }, cancellationToken);
                        }
                    }
                    //if service is sequence
                    else if (request.renewalConfig.IsSequence && String.IsNullOrEmpty(request.renewalConfig.ScheduleDay.ToString()))
                    {
                        NextRenewalDate = DateTime.Today.AddDays(request.renewalConfig.ScheduleSequence);
                        //send message to SMSOUTP Queue
                        foreach (var Subscription in listSubscription)
                        {
                            if (!String.IsNullOrEmpty(rMessage.MessageTxt))
                            {
                                string iQueue = "SMSOUTP";
                                var MtTxId = Guid.NewGuid().ToString("N");
                                await _mediator.Send(new SendSmsoutQueueCommand
                                {
                                    rMessage = rMessage,
                                    rMsisdn = Subscription.Msisdn,
                                    rSparam = String.Empty,
                                    rIparam = 0,
                                    rQueue = iQueue,
                                    rServiceId = Subscription.ServiceId,
                                    rMtTxId = MtTxId,
                                    QueueAuth = request.QueueAuth
                                });
                                MessageCount++;
                            }
                            //Update Subs for next Renewal Date
                            await _mediator.Send(new UpdateSubscriptionRenewal { subscription = Subscription, rNextRenewalDate = NextRenewalDate }, cancellationToken);
                        }
                    }
                }
                sw.Stop();
                renewVM.GenerateSpan = sw.Elapsed;
                renewVM.MessagesGenerated = MessageCount;
                return renewVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
