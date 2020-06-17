using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.SMS.RENEWAL.Queries
{
    public class GetNextRenewalDate : IRequest<DateTime>
    {
        public int rServiceid { get; set; }
        public int rOperatorid { get; set; }
    }
}
