using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.SMS.BLACKLIST.Query;
using Application.SMS.RENEWAL.Queries;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.SMS.SUBSCRIPTION.Commands
{
    public class InsertSubscriptionHandler : IRequestHandler<InsertSubscription, Subscription>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;

        public InsertSubscriptionHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<Subscription> Handle(InsertSubscription request, CancellationToken cancellationToken)
        {
            try
            {
                //Check if its on Blacklist
                if (!await _mediator.Send(new IsBlacklist { Msisdn = request.rMsisdn, OperatorId= request.rOperatorid}))
                {
                    //Get Next renewDate
                    var rNextRenewDate = await _mediator.Send(new GetNextRenewalDate
                    {
                        rServiceid = request.rServiceid,
                        rOperatorid = request.rOperatorid
                    }, cancellationToken);

                    var NewSubs = new Subscription()
                    {
                        Msisdn = request.rMsisdn,
                        Reg_Keyword = request.rMoMessage,
                        Subscription_Date = DateTime.Now,
                        Next_Renew_Time = rNextRenewDate,
                        Total_Revenue = 0,
                        Mt_Sent = 0,
                        Mt_Success = 0,
                        ServiceId = request.rServiceid,
                        OperatorId = request.rOperatorid,
                        //Shortcode = request.rShortcode
                    };

                    await _context.Subscriptions.AddAsync(NewSubs);
                    await _context.SaveChangesAsync(cancellationToken);

                    return NewSubs;
                }
                else
                {
                    //Throw Msisdn is blacklisted
                    throw new NotFoundException(nameof(BlackList), request.rMsisdn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
