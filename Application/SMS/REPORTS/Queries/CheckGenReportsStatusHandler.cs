using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.REPORTS.Queries
{
    public class CheckGenReportsStatusHandler : IRequestHandler<CheckGenReportsStatus, bool>
    {
        private readonly IRediSmsDbContext _context;
        public CheckGenReportsStatusHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CheckGenReportsStatus request, CancellationToken cancellationToken)
        {
            return await _context.GenReports.Select(s => s.Status).FirstOrDefaultAsync();
        }
    }
}
