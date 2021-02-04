using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SMSIN.Commands
{
    public class SmsinVm
    {
        public string Msisdn { get; set; }
        public string Mo_Message { get; set; }
        public string MotxId { get; set; }
        public int ServiceId { get; set; }
        public int OperatorId { get; set; }
        public int Shortcode { get; set; }
        public int Status { get; set; }
        public string trx_status { get; set; }
    }
}
