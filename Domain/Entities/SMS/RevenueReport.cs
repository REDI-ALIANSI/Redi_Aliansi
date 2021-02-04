using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class RevenueReport
    {
        public int RevenueReportId { get; set; }
        public int SDC { get; set; }
        public DateTime Date { get; set; }
        public string Sid { get; set; }
        public float Price { get; set; }
        public int Mt_Hits { get; set; }
        public int Mt_Sent { get; set; }
        public string Mt_Type { get; set; }
        public double Revenue { get; set; }
        public int ServiceId { get; set; }
        public int OperatorId { get; set; }
        public Service Service { get; set; }
        public Operator Operator { get; set; }
    }
}
