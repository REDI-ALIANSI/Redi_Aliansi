using MediatR;
using System;
using Domain.Entities.SMS;

namespace Application.SMS.SUBSCRIPTION.Commands
{
    public class UpdateSubscriptionRenewal : IRequest
    {
        public DateTime rNextRenewalDate { get; set; }
        public int rServiceId { get; set; }
        public int rOperatorId { get; set; }
        public string rMsisdn { get; set; }
    }
}
