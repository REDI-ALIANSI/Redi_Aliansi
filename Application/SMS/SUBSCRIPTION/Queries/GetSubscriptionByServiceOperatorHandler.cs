using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.SMS.SUBSCRIPTION.Queries
{
    public class GetSubscriptionByServiceOperatorHandler : IRequestHandler<GetSubscriptionByServiceOperator, List<Subscription>>
    {
        private readonly IRediSmsDbContext _context;

        public GetSubscriptionByServiceOperatorHandler(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subscription>> Handle(GetSubscriptionByServiceOperator request, CancellationToken cancellationToken)
        {
            return await _context.Subscriptions
                                    .Where(s => s.ServiceId.Equals(request.ServiceId) && s.OperatorId.Equals(request.OperatorId) && s.Next_Renew_Time.Equals(request.rRenewalDate))
                                    .AsNoTracking()
                                     .ToListAsync();
        }
    }
}
