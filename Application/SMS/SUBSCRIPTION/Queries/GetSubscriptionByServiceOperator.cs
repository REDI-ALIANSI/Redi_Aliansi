using Domain.Entities.SMS;
using MediatR;
using System;
using System.Collections.Generic;

namespace Application.SMS.SUBSCRIPTION.Queries
{
    public class GetSubscriptionByServiceOperator : IRequest<List<Subscription>>
    {
        public int ServiceId { get; set; }
        public int OperatorId { get; set; }
        public DateTime rRenewalDate { get; set; }
    }
}
