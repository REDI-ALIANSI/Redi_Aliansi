using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.SMS.SUBSCRIPTION.Commands
{
    public class UpdateSubscriptionRenewalHandler : IRequestHandler<UpdateSubscriptionRenewal>
    {
        private readonly IRediSmsDbContext _context;

        public UpdateSubscriptionRenewalHandler(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateSubscriptionRenewal request, CancellationToken cancellationToken)
        {
            var subscription = _context.Subscriptions.Where(s => s.Msisdn.Equals(request.rMsisdn)
                                                                && s.ServiceId.Equals(request.rServiceId)
                                                                && s.OperatorId.Equals(request.rOperatorId))
                                                                .FirstOrDefault();
            if(subscription != null)
            {
                subscription.Next_Renew_Time = request.rNextRenewalDate;
                _context.Subscriptions.Update(subscription);
                await _context.SaveChangesAsync(cancellationToken);
            }
                
            return Unit.Value;
        }
    }
}
