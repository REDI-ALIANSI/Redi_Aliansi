using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SMSDN.Commands
{
    public class ExcelDrRequest
    {
        public string _tid { get; set; }
        public string status_id { get; set; }
        public string dtdone { get; set; }
        public string errorcode { get; set; }
        public string errordescription { get; set; }
        public string sid { get; set; }
    }
}
