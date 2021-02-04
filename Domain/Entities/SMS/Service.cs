using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class Service : AuditableEntity
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ServiceCustom { get; set; }
        public bool IsActive { get; set; }
        public bool IsCustom { get; set; }

        public int Shortcode { get; set; }
        public int ServiceTypeId { get; set; }

        public ShortCode ShortCode { get; set; }
        public ServiceType ServiceType { get; set; }
        public virtual List<Keyword> Keywords { get; set; }
        public virtual List<Subscription> Subscriptions { get; set; }
        public virtual List<SubscriptionHist> GetSubscriptionHists { get; set; }
        public virtual List<ServiceCampaign> ServiceCampaigns { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual List<ServiceRenewalConfiguration> ServiceRenewalConfigurations { get; set; }
        public virtual List<RevenueReport> RevenueReports { get; set; } 
        public virtual List<SubscriptionReport> SubscriptionReports { get; set; }
    }
}
