using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.SMS
{
    public class Content : AuditableEntity
    {
        public int ContentId { get; set; }
        public string ContentText { get; set; }
        public string ContentPath { get; set; }
        public DateTime? ContentSchedule { get; set; }
        public bool Processed { get; set; }

        public int ContentTypeId { get; set; }
        public int MessageId { get; set; }

        public ContentType ContentType { get; set; }
        public Message Message { get; set; }
    }
}
