﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.SMS.SMSOUT.Queries
{
    public class GetSmsoutbyMttxidHandler : IRequestHandler<GetSmsoutbyMttxid, SmsoutD>
    {
        private readonly IRediSmsDbContext _context;

        public GetSmsoutbyMttxidHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<SmsoutD> Handle(GetSmsoutbyMttxid request, CancellationToken cancellationToken)
        {
            try
            {
                var SmsOut = await _context.SmsoutDs.Where(o => o.MtTxId.Equals(request.MttxId))
                    .Include(o => o.Service)
                    .Include(o => o.Operator)
                    .Include(o => o.Message)
                    .Include(o => o.Message.Sid)
                    .FirstOrDefaultAsync();
                return SmsOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
