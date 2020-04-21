using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class Message : AuditableEntity
    {
        public int MessageId { get; set; }
        public string MessageTxt { get; set; }
        public int Order { get; set; }
        public string Billing1 { get; set; }
        public string Billing2 { get; set; }
        public string Billing3 { get; set; }
        public bool IsDynamicBilling { get; set; }
        public bool IsRichContent { get; set; }
        public string MessageType { get; set; }
        public double Delay { get; set; }
        public bool IsDnWatch { get; set; }
        public string Sparam { get; set; }

        public string SidBilling { get; set; }
        public int OperatorId { get; set; }
        public int ServiceId { get; set; }
        

        public Sid Sid { get; set; }
        public Operator Operator { get; set; }
        public Service Service { get; set; }
        public List<ServiceRenewalConfiguration> ServiceRenewalConfigurations { get; set; }
    }
}
