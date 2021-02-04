using System;
using System.Collections.Generic;
using Application.Common.Model;
using Domain.Entities;
using Domain.Entities.SMS;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.SMS.CONTENT.ViewModel
{
    public class InsertContentVM
    {
        public string ContentText { get; set; }
        public DateTime? ContentSchedule { get; set; }
        public int? ContentTypeId { get; set; }
        public string ContentPath { get; set; }
        public int MessageOrder { get; set; }
        public int? ServiceId { get; set; }
        public int? OperatorId { get; set; }
        public SelectList ContentType { get; set; }
        public SelectList ListVMService { get; set; }
        public SelectList ListVMOperator { get; set; }
        public Result InsertResult { get; set; }
    }
}

