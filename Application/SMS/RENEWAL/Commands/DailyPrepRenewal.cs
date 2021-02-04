using System;
using System.Collections.Generic;
using System.Text;
using Application.SMS.RENEWAL.ViewModel;
using Common;
using MediatR;

namespace Application.SMS.RENEWAL.Commands
{
    public class DailyPrepRenewal : IRequest<List<RenewalVM>>
    {
        public DateTime RenewalTime { get; set; }
        public RabbitMQAuth QueueAuth { get; set; }
    }
}
