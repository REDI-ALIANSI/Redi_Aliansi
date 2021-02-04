using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Application.SMS.SERVICE.ViewModel
{
    public class vmCreateServiceWizard
    {
        public string Service_Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsCustom { get; set; }
        public string ServiceCustom { get; set; }

        public int ShortCode { get; set; }
        public SelectList lShortCode { get; set; }
        public int ServiceType { get; set; }
        public SelectList lServiceType { get; set; }
        public string Keyword { get; set; }
        public List<string> SubKeywords { get; set; }
    }
}