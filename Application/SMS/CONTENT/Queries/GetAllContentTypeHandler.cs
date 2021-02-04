using Application.Common.Interfaces;
using Application.SMS.CONTENT.Queries;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.CONTENT.Queries
{
    public class GetAllContentTypeHandler : IRequestHandler<GetAllContentType, List<ContentType>>
    {
        private readonly IRediSmsDbContext _context;
        public GetAllContentTypeHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<List<ContentType>> Handle(GetAllContentType request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.ContentTypes.ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
