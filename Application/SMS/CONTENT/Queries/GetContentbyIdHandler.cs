using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.CONTENT.Queries
{
    public class GetContentbyIdHandler : IRequestHandler<GetContentbyId, Content>
    {
        private readonly IRediSmsDbContext _context;
        public GetContentbyIdHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<Content> Handle(GetContentbyId request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Contents.Where(c => c.ContentId == request.ContentId).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
