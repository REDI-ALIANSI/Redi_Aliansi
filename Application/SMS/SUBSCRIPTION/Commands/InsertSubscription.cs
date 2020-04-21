using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.SUBSCRIPTION.Commands
{
    public class InsertSubscription : IRequest<Subscription>
    {
        public string rMsisdn { get; set; }
        public string rMoMessage { get; set; }
        public int rServiceid { get; set; }
        public int rOperatorid { get; set; }
        public int rShortcode { get; set; }
    }
}
