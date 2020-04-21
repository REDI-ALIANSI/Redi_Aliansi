
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class SubKeyword : AuditableEntity
    {
        public int SubKeywordId { get; set; }
        public string SubKeyWord { get; set; }
        public int KeywordId { get; set; }

        public Keyword Keyword { get; set; }
    }
}
