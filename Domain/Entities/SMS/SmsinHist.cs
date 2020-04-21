using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class SmsinHist
    {
        public int SmsinHistId { get; set; }
        public string Msisdn { get; set; }
        public string Mo_Message { get; set; }
        public string MotxId { get; set; }
        public DateTime DateCreated { get; set; }

        public int ServiceId { get; set; }
        public int OperatorId { get; set; }
        public int Shortcode { get; set; }

        public Service Service { get; set; }
        public Operator Operator { get; set; }
        public ShortCode ShortCode { get; set; }
    }
}
