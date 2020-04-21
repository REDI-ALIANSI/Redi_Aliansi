using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.SMS.SUBSCRIPTION.Commands
{
    public class UnregSubscriptionHandler : IRequestHandler<UnregSubscription>
    {
        private readonly IRediSmsDbContext _context;

        public UnregSubscriptionHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(UnregSubscription request, CancellationToken cancellationToken)
        {
            try
            {
                var subs = await _context.Subscriptions.Where(s => s.Equals(request.rSubscription)).FirstOrDefaultAsync();
                var subsHist = new SubscriptionHist()
                {
                    Last_Renew_Time = subs.Last_Renew_Time,
                    Msisdn = subs.Msisdn,
                    Reg_Keyword = subs.Reg_Keyword,
                    Unreg_keyword = request.rUnreg_keyword,
                    Subscription_Date = subs.Subscription_Date,
                    Unsubscription_Date = DateTime.Now,
                    State = request.rState,
                    Total_Revenue = subs.Total_Revenue,
                    Mt_Sent = subs.Mt_Sent,
                    Mt_Success = subs.Mt_Success,
                    ServiceId = subs.ServiceId,
                    OperatorId = subs.OperatorId,
                    //Shortcode = subs.Shortcode
                };
                await _context.SubscriptionHists.AddAsync(subsHist);
                _context.Subscriptions.Remove(subs);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
