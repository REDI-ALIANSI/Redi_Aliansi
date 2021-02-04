using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.SUBSCRIPTION.Queries
{
    public class GetSubscription : IRequest<Subscription>
    {
        public string Msisdn { get; set; }
        public int ServiceId { get; set; }
        public int OperatorId { get; set; }
    }

    public class GetSubscriptionHandler : IRequestHandler<GetSubscription, Subscription>
    {
        private readonly IRediSmsDbContext _context;

        public GetSubscriptionHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<Subscription> Handle(GetSubscription request, CancellationToken cancellationToken)
        {
            return await _context.Subscriptions.Where(s => s.Msisdn.Equals(request.Msisdn)
                                                && s.ServiceId.Equals(request.ServiceId)
                                                && s.OperatorId.Equals(request.OperatorId))
                                                .FirstOrDefaultAsync();
        }
    }
}
