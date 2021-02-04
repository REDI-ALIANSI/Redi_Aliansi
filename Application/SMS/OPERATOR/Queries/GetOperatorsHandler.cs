using Application.Common.Interfaces;
using Application.Common.Model;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.OPERATOR.Queries
{
    public class GetOperatorsHandler : IRequestHandler<GetOperators, List<Operator>>
    {
        private readonly IRediSmsDbContext _context;
        public GetOperatorsHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<List<Operator>> Handle(GetOperators request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Operators.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
