using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class ServiceCampaign : AuditableEntity
    {
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public bool IsMainKeyword { get; set; }
        public bool IsActive { get; set; }
        public bool IsCallBackRequired { get; set; }
        public DateTime ExpiredDate { get; set; }

        public string CampaignKeyword { get; set; }
        
        public int OperatorId { get; set; }
        public int ServiceId { get; set; }

        public Operator Operator { get; set; }
        public Service Service { get; set; }
    }
}
