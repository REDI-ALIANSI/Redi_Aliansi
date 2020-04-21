using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.SMS.RENEWAL.Commands
{
    public class DailyPrepRenewal : IRequest
    {
        public DateTime RenewalTime { get; set; }
    }
}
