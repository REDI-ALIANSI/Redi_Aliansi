using Application.Common.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.CONTENT.ViewModel
{
    public class EditContentVM
    {
        public string ContentText { get; set; }
        public DateTime? ContentSchedule { get; set; }
        public int? ContentTypeId { get; set; }
        public string ContentPath { get; set; }
        public int MessageOrder { get; set; }
        public int ContentId { get; set; }
        public int? ServiceId { get; set; }
        public int? OperatorId { get; set; }
        public SelectList ContentType { get; set; }
        public SelectList ListVMService { get; set; }
        public SelectList ListVMOperator { get; set; }
        public Result EditContentResult { get; set; }
    }
}
