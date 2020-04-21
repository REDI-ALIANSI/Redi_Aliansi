using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class ServiceType
    {
        public int ServiceTypeId { get; set; }
        public string Type { get; set; }

        public List<Service> Services { get; set; }
    }
}
