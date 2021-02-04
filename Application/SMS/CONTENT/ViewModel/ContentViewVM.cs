using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.SMS;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.SMS.CONTENT.ViewModel
{
    public class ContentViewVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? ServiceId { get; set; }
        public int? OperatorId { get; set; }
        public SelectList ListVMService { get; set; }
        public SelectList ListVMOperator { get; set; }
        public List<Content> Contents { get; set; }
    }
}
