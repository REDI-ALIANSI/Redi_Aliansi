using System;
using Common;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.RENEWAL.Commands
{
    public class ProcessRenewalConfig : IRequest
    {
        public ServiceRenewalConfiguration renewalConfig { get; set; }
        public DateTime rRenewalTime { get; set; }
        public int rRenewalConfigCount { get; set; }
        public RabbitMQAuth QueueAuth { get; set; }
    }
}
