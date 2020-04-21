using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Application.SMS.SMSOUT.Commands
{
    [XmlRoot("push-response")]
    public class XlResponseMessage
    {
        [XmlElement("tid")]
        public string tid
        {
            get;
            set;
        }

        [XmlElement("status-id")]
        public string status_id
        {
            get;
            set;
        }

        [XmlElement("message")]
        public string message
        {
            get;
            set;
        }

        [XmlElement("sdc")]
        public string sdc
        {
            get;
            set;
        }
    }
}
