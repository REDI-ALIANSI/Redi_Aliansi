using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.SMS.SERVICE.Queries;
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

        ProcessSmsDnWatchQueueHandler(IRediSmsDbContext context, IMsgQ msgQ, IMediator mediator)
        {
            _context = context;
            _msgQ = msgQ;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ProcessSmsDnWatchQueue request, CancellationToken cancellationToken)
        {
            try
            {
                string msgQueue = await _msgQ.ConsumerQueue(request.queue);
                if (!msgQueue.Equals("ERROR : NO MESSAGE FOUND"))
                {
                    var smsdnQ = JsonSerializer.Deserialize<SmsdnD>(msgQueue);
                    if (!smsdnQ.Equals(null))
                    {
                        //Check and get SMSOUTD for this DN
                        smsdnQ.SmsoutD = await _context.SmsoutDs.Where(o => o.MtTxId.Equals(smsdnQ.MtTxId))
                                            .Include(m => m.Message).SingleOrDefaultAsync();
                        //execute Service script :: HOMEWORK : How to log this?
                        var service = await _mediator.Send(new GetServiceById { ServiceId = smsdnQ.SmsoutD.Message.ServiceId }, cancellationToken);
                        //if (!String.IsNullOrEmpty(service.Dll))
                        //{
                        //    string DllPath = request.appsDllPath + service.Dll;
                        //    object[] dllParams = new object[] { smsdnQ };
                        //    var DllResult = ExecuteDllService.ProcessExecute(DllPath, service.Dll, "Dnwatch", dllParams);

                        //    if (!DllResult.Equals("200"))
                        //    {
                        //        await _msgQ.ProducerQueue(smsdnQ, request.queue);
                        //    }
                        //}
                        //else throw new NotFoundException(nameof(ServiceRenewalConfiguration), smsdnQ.SmsoutD.Message.ServiceId);
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
