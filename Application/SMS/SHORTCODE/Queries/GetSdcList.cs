using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.SHORTCODE.Queries
{
    public class GetSdcList : IRequest<List<ShortCode>>
    {
    }
    public class GetSdcListHander : IRequestHandler<GetSdcList, List<ShortCode>>
    {
        private readonly IRediSmsDbContext _context;

        public GetSdcListHander(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShortCode>> Handle(GetSdcList request, CancellationToken cancellationToken)
        {
            return await _context.ShortCodes.ToListAsync();
        }
    }
}
