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
            if(request.subscription != null)
            {
                request.subscription.Next_Renew_Time = request.rNextRenewalDate;
                _context.Subscriptions.Update(request.subscription);
                await _context.SaveChangesAsync(cancellationToken);
            }
                
            return Unit.Value;
        }
    }
}
