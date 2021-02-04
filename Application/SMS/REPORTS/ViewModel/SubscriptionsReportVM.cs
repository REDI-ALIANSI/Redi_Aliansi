using Domain.Entities.SMS;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Application.SMS.REPORTS.ViewModel
{
    public class SubscriptionsReportVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ServiceId { get; set; }
        public int OperatorId { get; set; }
        public int Sdc { get; set; }
        public SelectList ListVMService { get; set; }
        public SelectList ListVMOperator { get; set; }
        public SelectList ListSdc { get; set; }
        public List<SubscriptionReport> Subscriptionreports { get; set; }
    }
}
