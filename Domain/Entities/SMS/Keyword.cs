using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class Keyword : AuditableEntity
    {
        public int KeywordId { get; set; }
        public string KeyWord { get; set; }
        public int ServiceId { get; set; }

        public Service Service { get; set; }
        public virtual List<SubKeyword> SubKeywords { get; set; }
    }
}
