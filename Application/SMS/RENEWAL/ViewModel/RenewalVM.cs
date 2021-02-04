using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.RENEWAL.ViewModel
{
    public class RenewalVM
    {
        public string ServiceName { get; set; }
        public int MessagesGenerated { get; set; }
        public TimeSpan GenerateSpan { get; set; }
    }
}
