using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SMSDN.Commands
{
    public static class IndosatDnResponse
    {
        public static string GetResponse(string transid, string errorcode)
        {
            var xmlResponse = new StringBuilder();
            xmlResponse.Append("<?xml version=\"1.0\" ?>");
            xmlResponse.Append("<DR><STATUS>" + errorcode + "</STATUS>");
            xmlResponse.Append("<TRANSID>" + transid + "</TRANSID>");
            xmlResponse.Append("<MSG>Message processed successfully</MSG>");
            xmlResponse.Append("</DR>");

            return xmlResponse.ToString();
        }
    }
}
