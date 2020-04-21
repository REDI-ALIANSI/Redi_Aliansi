using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SMSOUT.Commands
{
    public class SmsoutVm
    {
        public string Msisdn { get; set; }
        public string Mt_Message { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateProcessed { get; set; }
        public string Trx_Status { get; set; }
        public bool IsDnWatch { get; set; }
        public string Sparam { get; set; }
        public int Iparam { get; set; }
        public string MtTxId { get; set; }
        public int OperatorId { get; set; }
        public int Shortcode { get; set; }
        public int MessageId { get; set; }
        public int ServiceId { get; set; }
        public int Status { get; set; }
        public string Response { get; set; }
        public string URI_Hit { get; set; }
    }
}
