using Application.Common.Interfaces;
using Application.Common.Model;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.CONTENT.Command
{
    public class DeleteContentHandler : IRequestHandler<DeleteContent, Result>
    {
        private readonly IRediSmsDbContext _context;

        public DeleteContentHandler(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteContent request, CancellationToken cancellationToken)
        {
            try
            {
                var content = await _context.Contents.Where(c => c.ContentId == request.ContentId).FirstOrDefaultAsync();
                if (content != null)
                {
                    _context.Contents.Remove(content);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success();
                }
                else
                {
                    var errorIEnurable = new List<string>() { "Content Not Found!" };
                    return Result.Failure(errorIEnurable);
                }
            }
            catch (Exception ex)
            {
                var errorIEnurable = new List<string>() { ex.Message };
                var resultExceltiop = Result.Failure(errorIEnurable);
                return resultExceltiop;
            }
        }
    }
}
