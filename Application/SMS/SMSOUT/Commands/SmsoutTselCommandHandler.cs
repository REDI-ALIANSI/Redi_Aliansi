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
    public class SmsoutTselCommandHandler : IRequestHandler<SmsoutTselCommand, SmsoutTelcoHitVm>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IHttpRequest _httpRequest;

        public SmsoutTselCommandHandler(IRediSmsDbContext context, IHttpRequest httpRequest)
        {
            _context = context;
            _httpRequest = httpRequest;
        }

        public async Task<SmsoutTelcoHitVm> Handle(SmsoutTselCommand request, CancellationToken cancellationToken)
        {
            try
            {
                SmsoutTelcoHitVm TselVm = new SmsoutTelcoHitVm();

                StringBuilder url = new StringBuilder();
                //get URL Operator
                string TselUrl = _context.Operators.Where(o => o.OperatorId.Equals(request.Smsout.OperatorId))
                                                .Select(o => o.UrlOut)
                                                .FirstOrDefault();

                //Build URL Tsel API
                url.Append(TselUrl);
                url.Append("?cpid=" + request.Smsout.Message.Billing1);
                url.Append("&pwd=" + request.Smsout.Message.Billing2);
                url.Append("&sid=" + request.Smsout.Message.SidBilling);
                url.Append("&sender=" + request.Smsout.Service.Shortcode);
                url.Append("&msisdn=" + request.Smsout.Msisdn);
                url.Append("&sms=" + request.Smsout.Mt_Message);
                url.Append("&trx_id=" + request.Smsout.MtTxId);

                //call TSEL API
                TselVm.ResponseRaw = await _httpRequest.GetRequest(url.ToString());
                TselVm.Response = TselVm.ResponseRaw;

                return TselVm;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
