using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SMSDN.Commands
{
    public class IndosatDnRequest
    {
        public string time { get; set; }
        public string serviceid { get; set; }
        public string dest { get; set; }
        public string tid { get; set; }
        public string status { get; set; }
    }
}
