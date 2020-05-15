using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Common.Exceptions;
using Application.Common.Behaviour;

namespace Application.SMS.SMSIN.Commands
{
    public class SendSmsinQueueCommandHandler : IRequestHandler<SendSmsinQueueCommand>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMsgQ _msgQ;
        private readonly IMediator _mediator;

        public SendSmsinQueueCommandHandler(IRediSmsDbContext context, IMsgQ msgQ, IMediator mediator)
        {
            _context = context;
            _msgQ = msgQ;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SendSmsinQueueCommand request, CancellationToken cancellationToken)
        {
            //string[] arrReserved = { "ON", "OFF", "STOP", "HELP", "REG", "UNREG", "INFO" };
            var ArrKeyword = request.Mo_Message.Split(' ');
            string keyword = request.Mo_Message;
            if (ArrKeyword.Length > 1)
            {
                if ( await _mediator.Send(new CheckReservedKeyword { Mo_Message = ArrKeyword[0] }, cancellationToken)) 
                    keyword = ArrKeyword[1].ToLower();
                else keyword = ArrKeyword[0].ToLower();
            }

            int KeywordServiceid = _context.Keywords.
                                        Where(k => k.KeyWord.Equals(keyword)).
                                        Select(s => s.ServiceId).SingleOrDefault();
            if(KeywordServiceid == 0)
            {
                var subKeyword = _context.SubKeywords.Where(k => k.SubKeyWord.Equals(keyword))
                                        .Include(i => i.Keyword)
                                        .SingleOrDefault();

                if (!subKeyword.Equals(null)) KeywordServiceid = subKeyword.Keyword.ServiceId;
                else
                {
                    KeywordServiceid = await _context.Services.Where(s => s.Description.Equals("ERROR")).Select(o => o.ServiceId).SingleOrDefaultAsync();
                }
            }

            SmsinD smsin = new SmsinD()
            {
                Msisdn = request.Msisdn,
                Mo_Message = request.Mo_Message,
                MotxId = request.Motxid,
                DateCreated = DateTime.Now,
                OperatorId = request.OperatorId,
                ServiceId = KeywordServiceid,
                //Shortcode = request.Shortcode
            };
            string QueueName = "SMSINQ";

            await _msgQ.ProducerQueue(smsin, QueueName, request.QueueAuth);

            return Unit.Value;
        }
    }
}
