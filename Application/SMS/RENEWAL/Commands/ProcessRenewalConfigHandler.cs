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

namespace Application.SMS.RENEWAL.Commands
{
    public class ProcessRenewalConfigHandler : IRequestHandler<ProcessRenewalConfig>
    {
        private readonly IMediator _mediator;
        private readonly IRediSmsDbContext _context;

        public ProcessRenewalConfigHandler(IMediator mediator, IRediSmsDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Unit> Handle(ProcessRenewalConfig request, CancellationToken cancellationToken)
        {
            try
            {
                DateTime NextRenewalDate = new DateTime();
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
                    var rMessage = request.renewalConfig.Message;
                    var rTxtMessage = await _mediator.Send(new GetRenewalMessage { rMessage = request.renewalConfig.Message, rRenewalDate = DateTime.Today },cancellationToken);
                    
                    //If renewal message is empty send Email warning notification here
                    if (!String.IsNullOrEmpty(rTxtMessage))
                    {
                        rMessage.MessageTxt = rTxtMessage;
                    }
                    
                    // Make function for put renewal message to SMSOUTD Queue
                    if (request.renewalConfig.ActiveDll)
                    {
                        //Check Dll here
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
                            }
                            //Update Subs for next Renewal Date
                            await _mediator.Send(new UpdateSubscriptionRenewal { subscription = Subscription, rNextRenewalDate = NextRenewalDate }, cancellationToken);
                        }
                    }
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
