using Domain.Entities.SMS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SMSDN.Commands
{
    public class SendSmsDnWatchQueue : IRequest
    {
        public SmsdnD smsdnD { get; set; }
    }
}
