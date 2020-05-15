using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.SMSOUT.Commands
{
    public class SmsoutXlCommandHandler : IRequestHandler<SmsoutXlCommand, SmsoutTelcoHitVm>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IHttpRequest _httpRequest;

        public SmsoutXlCommandHandler(IRediSmsDbContext context, IHttpRequest httpRequest)
        {
            _context = context;
            _httpRequest = httpRequest;
        }
        public async Task<SmsoutTelcoHitVm> Handle(SmsoutXlCommand request, CancellationToken cancellationToken)
        {
            try
            {
                SmsoutTelcoHitVm XlVm = new SmsoutTelcoHitVm();

                StringBuilder url = new StringBuilder();
                //get URL Operator
                string XltUrl = _context.Operators.Where(o => o.OperatorId.Equals(request.Smsout.OperatorId))
                                                .Select(o => o.UrlOut)
                                                .FirstOrDefault();

                //Build XL URL
                url.Append(XltUrl);
                url.Append("?APP_PWD=" + request.Smsout.Message.Billing1);
                url.Append("&DEST_ADDR=" + request.Smsout.Msisdn);
                url.Append("&PROTOCOL_ID=0");
                url.Append("&MSG_CLASS=1");
                url.Append("&TEXT=" + request.Smsout.Mt_Message);
                url.Append("&REGISTERED=yes");
                url.Append("&SOURCE_ADDR=" + request.Smsout.Service.Shortcode);
                url.Append("&SHORT_NAME=" + request.Smsout.Message.SidBilling);
                url.Append("&APP_ID=" + request.Smsout.Message.Billing2);
                if (request.Smsout.Message.MessageType.Equals("PULL"))
                {
                    url.Append("&TX_ID=" + request.Smsout.MtTxId);
                }

                //call XL API
                XlVm.ResponseRaw = await _httpRequest.GetRequest(url.ToString());
                //Deserialize XML
                XlResponseMessage resp;
                using (TextReader reader = new StringReader(XlVm.ResponseRaw))
                {
                    resp = (XlResponseMessage)new XmlSerializer(typeof(XlResponseMessage)).Deserialize(reader);
                }
                XlVm.Response = resp.tid;

                XlVm.URI_Hit = url.ToString();

                //return HTTP Request
                return XlVm;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
