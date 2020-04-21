using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class SubscriptionHist
    {
        public int SubscriptionHistId { get; set; }
        public string Msisdn { get; set; }
        public string Reg_Keyword { get; set; }
        public string Unreg_keyword { get; set; }
        public DateTime Subscription_Date { get; set; }
        public DateTime Unsubscription_Date { get; set; }
        public string State { get; set; }
        public DateTime? Last_Renew_Time { get; set; }
        public float Total_Revenue { get; set; }
        public int Mt_Sent { get; set; }
        public int Mt_Success { get; set; }


        public int ServiceId { get; set; }
        public int OperatorId { get; set; }

        public Service Service { get; set; }
        public Operator Operator { get; set; }
    }
}
