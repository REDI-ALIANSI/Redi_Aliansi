using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class SmsdnD
    {
        public int SmsdnDId { get; set; }
        public string ErrorCode { get; set; }
        public string Status { get; set; }
        public string ErrorDesc { get; set; }

        public string MtTxId { get; set; }

        public SmsoutD SmsoutD { get; set; }
    }
}
