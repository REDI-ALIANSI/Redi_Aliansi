using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.MESSAGE.Command
{
    public class GetRenewalMessage : IRequest<string>
    {
        public Message rMessage { get; set; }
        public DateTime rRenewalDate { get; set; }
    }
}
