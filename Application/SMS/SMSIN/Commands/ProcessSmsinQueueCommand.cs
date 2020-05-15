using Domain.Entities.SMS;
using MediatR;
using Common;

namespace Application.SMS.SMSIN.Commands
{
    public class ProcessSmsinQueueCommand : IRequest<SmsinVm>
    {
        public string queue { get; set; }
        public string  appsDllPath { get; set; }
        public RabbitMQAuth QueueAuth { get; set; }
    }
}
