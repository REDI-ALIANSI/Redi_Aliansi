using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.SMS.SERVICE.Queries
{
    public class GetServiceByIdHandler : IRequestHandler<GetServiceById, Service>
    {
        private readonly IRediSmsDbContext _context;
        public GetServiceByIdHandler(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<Service> Handle(GetServiceById request, CancellationToken cancellationToken)
        {
            return await _context.Services.Where( s => s.ServiceId.Equals(request.ServiceId)).FirstOrDefaultAsync();
        }
    }
}
