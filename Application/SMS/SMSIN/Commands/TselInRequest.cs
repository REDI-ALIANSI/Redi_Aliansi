using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SMSIN.Commands
{
    public class TselInRequest
    {
        public string msisdn { get; set; }
        public string sms { get; set; }
        public string trx_id { get; set; }
        public string adn { get; set; }
    }
}
