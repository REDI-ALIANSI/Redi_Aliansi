using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.KEYWORD.Query
{
    public class GetKeywordByServiceid : IRequest<Keyword>
    {
        public int ServiceId { get; set; }
    }

    public class GetKeywordByServiceidHandler : IRequestHandler<GetKeywordByServiceid, Keyword>
    {
        private readonly IRediSmsDbContext _context;

        public GetKeywordByServiceidHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<Keyword> Handle(GetKeywordByServiceid request, CancellationToken cancellationToken)
        {
            return await _context.Keywords.Where(k => k.ServiceId.Equals(request.ServiceId)).FirstOrDefaultAsync();
        }
    }
}
