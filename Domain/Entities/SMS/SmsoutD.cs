using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class SmsoutD
    {
        public int SmsoutDId { get; set; }
        public string Msisdn { get; set; }
        public string Mt_Message { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateToProcessed { get; set; }
        public DateTime? DateProcessed { get; set; }
        public string Trx_Status { get; set; }
        public bool IsDnWatch { get; set; }
        public string Sparam { get; set; }
        public int Iparam { get; set; }
        public string MtTxId { get; set; }
        
        public int OperatorId { get; set; }
        public int MessageId { get; set; }
        public int ServiceId { get; set; }

        public Message Message { get; set; }
        public SmsdnD SmsdnD { get; set; }
        public Operator Operator { get; set; }
        public Service Service { get; set; }
    }
}
