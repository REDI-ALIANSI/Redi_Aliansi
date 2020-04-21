using Domain.Entities.SMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Operator
    {
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public int RetryLimit { get; set; }
        public string UrlIn { get; set; }
        public string UrlOut { get; set; }

        public List<Subscription> Subscriptions { get; set; }
        public List<SubscriptionHist> SubscriptionHists { get; set; }
        public List<ServiceCampaign> ServiceCampaigns { get; set; }
        public List<Message> Messages { get; set; }
    }
}
