using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class Sid : AuditableEntity
    {
        public string SidBilling { get; set; }

        public int OperatorId { get; set; }
        public float Price { get; set; }

        public virtual List<Message> Messages { get; set; }
    }
}
