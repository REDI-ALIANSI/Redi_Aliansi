using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SMSOUT.Commands
{
    public class XlSmsoutConReq
    {
        public string APP_PWD { get; set; }
        public string DEST_ADDR { get; set; }
        public string PROTOCOL_ID { get; set; }
        public string MSG_CLASS { get; set; }
        public string TEXT { get; set; }
        public string REGISTERED { get; set; }
        public string SOURCE_ADDR { get; set; }
        public string SHORT_NAME { get; set; }
        public string APP_ID { get; set; }
        public string TX_ID { get; set; }
    }
}
