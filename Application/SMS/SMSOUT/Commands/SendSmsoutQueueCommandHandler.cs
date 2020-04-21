using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.SMSOUT.Commands
{
    public class SendSmsoutQueueCommandHandler : IRequestHandler<SendSmsoutQueueCommand>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMsgQ _msgQ;

        public SendSmsoutQueueCommandHandler(IRediSmsDbContext context, IMsgQ msgQ, IMediator mediator)
        {
            _context = context;
            _msgQ = msgQ;
        }

        public async Task<Unit> Handle(SendSmsoutQueueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.rMessage != null)
                {
                    SmsoutD smsoutD = new SmsoutD()
                    {
                        Msisdn = request.rMsisdn,
                        Mt_Message = request.rMessage.MessageTxt,
                        DateCreated = DateTime.Now,
                        DateToProcessed = DateTime.Now.AddSeconds(request.rMessage.Delay),
                        DateProcessed = null,
                        Trx_Status = "100",
                        IsDnWatch = request.rMessage.IsDnWatch,
                        Shortcode = request.rMessage.Service.Shortcode,
                        OperatorId = request.rMessage.OperatorId,
                        Sparam = request.rSparam,
                        Iparam = request.rIparam,
                        MessageId = request.rMessage.MessageId,
                        ServiceId = request.rServiceId,
                        MtTxId = request.rMtTxId
                    };

                    await _msgQ.ProducerQueue(smsoutD, request.rQueue);
                }
                else
                {
                    throw new NotFoundException(nameof(Message), request.rMessage);
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
