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
    public class GetMessagesByTypeHandler : IRequestHandler<GetMessagesByType, List<Message>>
    {
        private readonly IRediSmsDbContext _context;

        public GetMessagesByTypeHandler(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> Handle(GetMessagesByType request, CancellationToken cancellationToken)
        {
            return await _context.Messages
                                    .Where(m => m.MessageType.Equals(request.MessageType)
                                     && m.ServiceId.Equals(request.Serviceid)
                                     && m.OperatorId.Equals(request.OperatorId))
                                     .OrderBy(m => m.Order)
                                     .ToListAsync();
        }
    }
}
