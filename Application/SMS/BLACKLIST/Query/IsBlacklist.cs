using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.BLACKLIST.Query
{
    public class IsBlacklist : IRequest<bool>
    {
        public string Msisdn { get; set; }
        public int OperatorId { get; set; }
    }

    public class IsBlacklistHandler : IRequestHandler<IsBlacklist, bool>
    {
        private readonly IRediSmsDbContext _context;
        public IsBlacklistHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(IsBlacklist request, CancellationToken cancellationToken)
        {

            var isBlacklist = await _context.BlackLists.Where(b => b.Msisdn.Equals(request.Msisdn) 
                                                        && b.OperatorId.Equals(request.OperatorId))
                                                        .AnyAsync();

            return isBlacklist;
        }
    }
}
