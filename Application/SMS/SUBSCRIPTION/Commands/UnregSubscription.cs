using Domain.Entities.SMS;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Application.Common.Model;

namespace Application.SMS.SUBSCRIPTION.Commands
{
    public class UnregSubscription : IRequest<Result>
    {
        public Subscription rSubscription {get; set;}
        public string rUnreg_keyword { get; set; }
        public string rState { get; set; }
    }
}
