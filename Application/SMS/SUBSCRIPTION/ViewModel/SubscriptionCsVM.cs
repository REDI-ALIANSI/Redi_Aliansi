using Domain.Entities;
using Domain.Entities.SMS;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SUBSCRIPTION.ViewModel
{
    public class SubscriptionsCsVMList
    {
        public float TotalCharged { get; set; }
        public DateTime Subscrition_Date { get; set; }
        public DateTime? Unsubscription_Date { get; set; }
        public int ServiceId { get; set; }
        public Service Iservice { get; set; }
    }
    public class SubscriptionCsVM
    {
        public string Msisdn { get; set; }
        public int OperatorId { get; set; }
        public bool IsBlacklisted { get; set; }
        public Operator iOperator { get; set; }
        public float Total_Charged { get; set; }
        public virtual List<SubscriptionsCsVMList> Subscriptions { get; set; }
        public SelectList ListVMOperator { get; set; }
    }
}
