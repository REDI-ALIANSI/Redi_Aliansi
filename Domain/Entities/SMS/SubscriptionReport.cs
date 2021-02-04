using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class SubscriptionReport
    {
        public int SubscriptionReportId { get; set; }
        public string ServiceName { get; set; }
        public int SDC { get; set; }
        public DateTime Date { get; set; }
        public int New_Member { get; set; }
        public int Churn_Member { get; set; }
        public int Total_Member { get; set; }
        public int ServiceId { get; set; }
        public int OperatorId { get; set; }
        public Operator Operator { get; set; }
        public Service Service { get; set; }
    }
}
