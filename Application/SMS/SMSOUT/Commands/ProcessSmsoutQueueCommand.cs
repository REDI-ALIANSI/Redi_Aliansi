using Domain.Entities.SMS;
using MediatR;
using System.Collections.Generic;
using Common;

namespace Application.SMS.SMSOUT.Commands
{
    public class ProcessSmsoutQueueCommand : IRequest<SmsoutVm>
    {
        public string Queue { get; set; }
        public RabbitMQAuth QueueAuth { get; set; }
    }
}
