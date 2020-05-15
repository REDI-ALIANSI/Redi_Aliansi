using Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SMSDN.Commands
{
    public class ProcessSmsDnWatchQueue : IRequest
    {
        public string queue { get; set; }
        public string appsDllPath { get; set; }
        public RabbitMQAuth QueueAuth { get; set; }
    }
}
