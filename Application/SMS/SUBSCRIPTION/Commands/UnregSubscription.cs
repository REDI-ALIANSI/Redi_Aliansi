using Domain.Entities.SMS;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.SMS.SUBSCRIPTION.Commands
{
    public class UnregSubscription : IRequest
    {
        public Subscription rSubscription {get; set;}
        public string rUnreg_keyword { get; set; }
        public string rState { get; set; }
    }
}
