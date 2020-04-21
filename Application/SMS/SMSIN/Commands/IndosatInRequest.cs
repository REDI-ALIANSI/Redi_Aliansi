using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.SMS.SMSIN.Commands
{
    public class IndosatInRequest
    {
        public string msisdn { get; set; }
        public string sms { get; set; }
        public string trx_time { get; set; }
        public string sc { get; set; }
        public string transid { get; set; }
        public string sdmsubsid { get; set; }
    }
}
