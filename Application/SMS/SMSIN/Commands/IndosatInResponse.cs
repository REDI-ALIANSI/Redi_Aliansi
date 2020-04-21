using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace Application.SMS.SMSIN.Commands
{
    public static class IndosatInResponse
    {
        public static string GetResponse(string transid)
        {
            var xmlResponse = new StringBuilder();
            xmlResponse.Append("<?xml version=\"1.0\" ?>");
            xmlResponse.Append("<MO><STATUS>0</STATUS>");
            xmlResponse.Append("<TRANSID>" + transid + "</TRANSID>");
            xmlResponse.Append("<MSG>Message processed successfully</MSG>");
            xmlResponse.Append("</MO>");

            return xmlResponse.ToString();
        }
    }
}
