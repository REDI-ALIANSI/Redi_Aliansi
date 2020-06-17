using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class SmsdnHist
    {
        public int SmsdnHistId { get; set; }
        public string ErrorCode { get; set; }
        public string Status { get; set; }
        public string ErrorDesc { get; set; }
        public DateTime DateInserted { get; set; }

        public string MtTxId { get; set; }

        public SmsoutHist SmsoutHist { get; set; }
    }
}
