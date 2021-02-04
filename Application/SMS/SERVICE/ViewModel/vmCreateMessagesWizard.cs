using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Application.SMS.SERVICE.ViewModel
{
    public class vmCreateMessagesWizard
    {
        public string Text { get; set; }
        public int MessageOrder { get; set; }
        public int Operator { get; set; }
        public SelectList lOperator { get; set; }
        public string SidBilling { get; set; }
        public SelectList lSidBilling { get; set; }
        public bool IsDynamicBilling { get; set; }
        public bool IsRichContent { get; set; }
        public string MessageType { get; set; }
        public double Delay { get; set; }
        public bool IsDnWatch { get; set; }
        public string Sparam { get; set; }
    }
}
