using System;
using System.Collections.Generic;
using Domain.Entities.SMS;
using Microsoft.EntityFrameworkCore;
using Application.Common.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Application.Common.Interfaces;
using MediatR;

namespace Application.SMS.REPORTS.Commands
{
    public class UpdateGenReportStatusHandler : IRequestHandler<UpdateGenReportStatus, Result>
    {
        private readonly IRediSmsDbContext _context;
        public UpdateGenReportStatusHandler(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateGenReportStatus request, CancellationToken cancellationToken)
        {
            var retResult = Result.Success();

            try
            {
                var genReportStatus = await _context.GenReports.Where(r => r.StatusId.Equals(1)).FirstOrDefaultAsync();
                genReportStatus.Status = request.StatusUpdate;
                _context.GenReports.Update(genReportStatus);
                await _context.SaveChangesAsync(cancellationToken);

                return retResult;
            }
            catch (Exception ex)
            {
                var Error = new List<string>() { ex.Message };
                retResult = Result.Failure(Error);
                return retResult;
            }
        }
    }
}
