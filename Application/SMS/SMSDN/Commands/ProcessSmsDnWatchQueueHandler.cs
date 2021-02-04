using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.SMS.SERVICE.Queries;
using Application.SMS.SERVICE.ViewModel;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.SMSDN.Commands
{
    public class ProcessSmsDnWatchQueueHandler : IRequestHandler<ProcessSmsDnWatchQueue>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMsgQ _msgQ;
        private readonly IMediator _mediator;
        private readonly IHttpRequest _httpRequest;

        public ProcessSmsDnWatchQueueHandler(IRediSmsDbContext context, IMsgQ msgQ, IMediator mediator, IHttpRequest httpRequest)
        {
            _context = context;
            _msgQ = msgQ;
            _mediator = mediator;
            _httpRequest = httpRequest;
        }

        public async Task<Unit> Handle(ProcessSmsDnWatchQueue request, CancellationToken cancellationToken)
        {
            try
            {
                string msgQueue = await _msgQ.ConsumerQueue(request.queue, request.QueueAuth);
                if (!msgQueue.Equals("ERROR : NO MESSAGE FOUND"))
                {
                    var smsdnQ = JsonSerializer.Deserialize<SmsdnD>(msgQueue);
                    if (!smsdnQ.Equals(null))
                    {
                        //Check and get SMSOUTD for this DN
                        smsdnQ.SmsoutD = await _context.SmsoutDs.Where(o => o.MtTxId.Equals(smsdnQ.MtTxId))
                                            .Include(m => m.Message).SingleOrDefaultAsync();
                        
                        //execute Custom Service script
                        var service = await _mediator.Send(new GetServiceById { ServiceId = smsdnQ.SmsoutD.Message.ServiceId }, cancellationToken);
                        
                        if (!String.IsNullOrEmpty(service.ServiceCustom))
                        {
                            string Uri = service.ServiceCustom = "/DnWatch";
                            var ReqObj = new CustomServiceDnwatchRequest() { smsdn = smsdnQ };
                            var Resp = await _httpRequest.PostHttpResp(Uri, ReqObj);
                            var RespCustApi = JsonSerializer.Deserialize<CustomServiceSmsinResponse>(Resp);
                            if (RespCustApi.result.ToUpper().Equals("OK"))
                            {
                                
                            }
                        }
                    }
                }
                else
                {
                    throw new NotFoundException(nameof(SmsdnD), msgQueue);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
