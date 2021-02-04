using Application.Common.Interfaces;
using Application.Common.Model;
using Domain.Entities;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.SERVICE.Queries
{
    public class GetServicesHandler : IRequestHandler<GetServices, List<Service>>
    {
        private readonly IRediSmsDbContext _context;
        public GetServicesHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<List<Service>> Handle(GetServices request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Services.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
