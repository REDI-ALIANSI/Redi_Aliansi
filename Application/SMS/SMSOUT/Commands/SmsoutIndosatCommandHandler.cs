using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.SMSOUT.Commands
{
    public class SmsoutIndosatCommandHandler : IRequestHandler<SmsoutIndosatCommand, SmsoutTelcoHitVm>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IHttpRequest _httpRequest;

        SmsoutIndosatCommandHandler(IRediSmsDbContext context, IHttpRequest httpRequest)
        {
            _context = context;
            _httpRequest = httpRequest;
        }

        public async Task<SmsoutTelcoHitVm> Handle(SmsoutIndosatCommand request, CancellationToken cancellationToken)
        {
            try
            {
                SmsoutTelcoHitVm IsatVm = new SmsoutTelcoHitVm();
                StringBuilder url = new StringBuilder();
                //Get Isat OUT URL
                string IsatUrl = _context.Operators.Where(o => o.OperatorId.Equals(request.Smsout.OperatorId))
                                            .Select(o => o.UrlOut)
                                            .FirstOrDefault();
                //Build ISAR URL GET API
                url.Append(IsatUrl);
                url.Append("?uid=" + request.Smsout.Message.Billing1);
                url.Append("&pwd=" + request.Smsout.Message.Billing2);
                url.Append("&serviceid=" + request.Smsout.Message.SidBilling);
                url.Append("&msisdn=" + request.Smsout.Msisdn);
                url.Append("&sms=" + request.Smsout.Mt_Message);
                url.Append("&transid=" + request.Smsout.MtTxId);
                url.Append("&smstype=0");
                url.Append("&sdmcode=" + request.Smsout.Message.Billing3);

                //Get HTTP 
                IsatVm.ResponseRaw = await _httpRequest.GetRequest(url.ToString());
                IsatVm.Response = "OK";

                IsatVm.URI_Hit = url.ToString();

                //return HTTP Request
                return IsatVm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
