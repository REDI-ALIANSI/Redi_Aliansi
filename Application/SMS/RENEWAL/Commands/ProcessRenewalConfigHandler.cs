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

                if(listSubscription != null)
                {
                    var today = DateTime.Today.DayOfWeek;
                    //GET Renewal Message from content
                    var rMessage = await _mediator.Send(new GetRenewalMessage { rMessage = request.renewalConfig.Message },cancellationToken);
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
                        foreach (var subscription in listSubscription)
                        {
                            string iQueue = "SMSOUTP";
                            await _mediator.Send(new SendSmsoutQueueCommand
                            {
                                rMessage = rMessage,
                                rMsisdn = subscription.Msisdn,
                                rSparam = String.Empty,
                                rIparam = 0,
                                rQueue = iQueue
                            });
                            //Update Subs for next Renewal Date
                            await _mediator.Send(new UpdateSubscriptionRenewal { rMsisdn = subscription.Msisdn, rNextRenewalDate = NextRenewalDate, rOperatorId = subscription.OperatorId, rServiceId = subscription.ServiceId },cancellationToken);
                        }
                    }
                    //if service is sequence
                    else if (request.renewalConfig.IsSequence && String.IsNullOrEmpty(request.renewalConfig.ScheduleDay.ToString()))
                    {
                        NextRenewalDate = DateTime.Today.AddDays(request.renewalConfig.ScheduleSequence);

                        //send message to SMSOUTP Queue
                        foreach (var subscription in listSubscription)
                        {
                            string iQueue = "SMSOUTP";
                            await _mediator.Send(new SendSmsoutQueueCommand
                            {
                                rMessage = rMessage,
                                rMsisdn = subscription.Msisdn,
                                rSparam = String.Empty,
                                rIparam = 0,
                                rQueue = iQueue
                            });
                            //Update Subs for next Renewal Date
                            await _mediator.Send(new UpdateSubscriptionRenewal { rMsisdn = subscription.Msisdn, rNextRenewalDate = NextRenewalDate, rOperatorId = subscription.OperatorId, rServiceId = subscription.ServiceId },cancellationToken);
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
