using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class ServiceRenewalConfiguration
    {
        public int ServiceRenewalConfigurationId { get; set; }
        public DayOfWeek? ScheduleDay { get; set; }
        public int ScheduleSequence { get; set; }
        public int ScheduleOrder { get; set; }
        public bool ActiveDll { get; set; }
        public bool IsActive { get; set; }

        public int ServiceId { get; set; }
        public int MessageId { get; set; }
        public int OperatorId { get; set; }
        public bool IsSequence { get; set; }

        public Service Service { get; set; }
        public Message Message { get; set; }
        public Operator Operator { get; set; }
    }
}
