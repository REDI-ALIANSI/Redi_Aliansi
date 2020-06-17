using System;
using System.Collections.Generic;
using System.Text;
using Common;
using MediatR;

namespace Application.SMS.RENEWAL.Commands
{
    public class DailyPrepRenewal : IRequest
    {
        public DateTime RenewalTime { get; set; }
        public RabbitMQAuth QueueAuth { get; set; }
    }
}
