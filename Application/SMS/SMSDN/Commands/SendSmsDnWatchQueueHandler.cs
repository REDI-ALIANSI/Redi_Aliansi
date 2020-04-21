using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Common.Exceptions;
using System.Text.Json;
using Application.Common.Behaviour;
using System.Collections.Generic;
using Application.SMS.MESSAGE.Queries;
using Application.SMS.SERVICE.Queries;

namespace Application.SMS.SMSDN.Commands
{
    public class SendSmsDnWatchQueueHandler : IRequestHandler<SendSmsDnWatchQueue>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMsgQ _msgQ;

        public SendSmsDnWatchQueueHandler(IRediSmsDbContext context, IMsgQ msgQ)
        {
            _context = context;
            _msgQ = msgQ;
        }

        public async Task<Unit> Handle(SendSmsDnWatchQueue request, CancellationToken cancellationToken)
        {
            try
            {
                string QueueName = "SMSDNWATCH";
                var smsdnd = request.smsdnD;
                //smsdnd.SmsoutD = request.smsoutD;

                if (smsdnd != null || smsdnd.SmsoutD != null || smsdnd.SmsoutD.Message != null)
                {
                    if (smsdnd.SmsoutD.Message.IsDnWatch)
                    {
                        //put SMSDND to QUEUE SMSDNWATCH
                        await _msgQ.ProducerQueue(smsdnd, QueueName);
                    }
                    else if (smsdnd.SmsoutD.Message.IsDynamicBilling)
                    {
                        //TODO NEXT : put Dynamic billing logic here!
                    }

                    //Add to SMSDND table
                    _context.SmsdnDs.Add(smsdnd);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    throw new NotFoundException(nameof(SmsdnD), smsdnd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Unit.Value;
        }
    }
}
