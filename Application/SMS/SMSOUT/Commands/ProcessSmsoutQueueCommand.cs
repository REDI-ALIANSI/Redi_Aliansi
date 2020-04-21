using Domain.Entities.SMS;
using MediatR;
using System.Collections.Generic;

namespace Application.SMS.SMSOUT.Commands
{
    public class ProcessSmsoutQueueCommand : IRequest<SmsoutVm>
    {
        public string Queue { get; set; }
    }
}
