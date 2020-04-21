using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Common.Exceptions;
using Application.SMS.SMSOUT.Queries;

namespace Application.SMS.SMSDN.Commands
{
    public class InsertDnRequestHandler : IRequestHandler<InsertDnRequest>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;

        public InsertDnRequestHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<Unit> Handle(InsertDnRequest request, CancellationToken cancellationToken)
        {
            try 
            {
                //Put to SMSDND object
                var SmsDn = new SmsdnD();
                SmsDn.ErrorCode = request.DnErrorcode;
                SmsDn.Status = request.Status;
                SmsDn.ErrorDesc = String.Empty;
                SmsDn.MtTxId = request.DnMtid;

                //GET SMSOUT CEK IF it needed DN Watcher
                var SmsOut = await _mediator.Send(new GetSmsoutbyMttxid { MttxId = request.DnMtid});
                SmsDn.SmsoutD = SmsOut;
                //Check and put DN if it on DN Watcher
                if (!(SmsOut is null))
                {
                    if(SmsOut.IsDnWatch) await _mediator.Send(new SendSmsDnWatchQueue { smsdnD = SmsDn });
                }
                
                //Save SMSDND
                await _context.SmsdnDs.AddAsync(SmsDn);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Unit.Value;
        }
    }
}
