using Domain.Entities.SMS;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Application.SMS.REPORTS.ViewModel
{
    public class RevenueReportVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? ServiceId { get; set; }
        public int? OperatorId { get; set; }
        public int? Sdc { get; set; }
        public int? Type { get; set; }
        public SelectList ListVMService { get; set; }
        public SelectList ListVMOperator { get; set; }
        public SelectList ListSdc { get; set; }
        public SelectList ListType { get; set; }
        public List<RevenueReport> Revenuereports { get; set; }
    }
}
