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
    public class GetMessagesbyId : IRequest<Message>
    {
        public int MessageId { get; set; }
    }

    public class GetMessagesbyIdHandler : IRequestHandler<GetMessagesbyId, Message>
    {
        private readonly IRediSmsDbContext _context;

        public GetMessagesbyIdHandler(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<Message> Handle(GetMessagesbyId request, CancellationToken cancellationToken)
        {
            return await _context.Messages.Where(m => m.MessageId.Equals(request.MessageId)).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
