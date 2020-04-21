using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.MESSAGE.Command
{
    public class GetRenewalMessage : IRequest<Message>
    {
        public Message rMessage { get; set; }
    }
}
