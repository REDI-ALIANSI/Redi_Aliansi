using Domain.Entities.SMS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SMSDN.Commands
{
    public class InsertDnRequest : IRequest
    {
        public string DnErrorcode { get; set; }
        public string DnMtid { get; set; }
        public string Status { get; set; }
    }
}
