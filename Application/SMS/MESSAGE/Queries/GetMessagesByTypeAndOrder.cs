using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.SMS.MESSAGE.Queries
{
    public class GetMessagesByTypeAndOrder : IRequest<Message>
    {
        public string MessageType { get; set; }
        public int Serviceid { get; set; }
        public int OperatorId { get; set; }
        public int Order { get; set; }
    }

    public class GetMessagesByTypeAndOrderHandler : IRequestHandler<GetMessagesByTypeAndOrder, Message>
    {
        private readonly IRediSmsDbContext _context;

        public GetMessagesByTypeAndOrderHandler(IRediSmsDbContext context)
        {
            _context = context;
        }


        public async Task<Message> Handle(GetMessagesByTypeAndOrder request, CancellationToken cancellationToken)
        {
            return await _context.Messages
                                    .Where(m => m.MessageType.Equals(request.MessageType)
                                     && m.ServiceId.Equals(request.Serviceid)
                                     && m.OperatorId.Equals(request.OperatorId)
                                     && m.Order.Equals(request.Order))
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync();
        }
    }
}
