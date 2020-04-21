using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class ShortCode : AuditableEntity
    {
        public int Shortcode { get; set; }
        public string Description { get; set; }

        public List<Service> Services {get; set;}
    }
}
