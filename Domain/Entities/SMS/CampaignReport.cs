using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class CampaignReport
    {
        public int CampaignReportId { get; set; }
        public DateTime Date { get; set; }
        public int New_Member { get; set; }
        public int Churn_Member { get; set; }
        public int Total_Member { get; set; }
        public int Mt_Sent { get; set; }
        public int Mt_Hits { get; set; }
        public double Revenue { get; set; }
        public int ServiceCampaignId { get; set; }
        public ServiceCampaign ServiceCampaign { get; set; }
    }
}
