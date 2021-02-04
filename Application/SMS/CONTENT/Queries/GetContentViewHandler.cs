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

namespace Application.SMS.CONTENT.Queries
{
    public class GetContentViewHandler : IRequestHandler<GetContentView, List<Content>>
    {
        private readonly IRediSmsDbContext _context;
        public GetContentViewHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<List<Content>> Handle(GetContentView request, CancellationToken cancellationToken)
        {
            try
            {
                //Get Filtered Content
                if (request.ViewVM.ServiceId == 0 && request.ViewVM.OperatorId == 0)
                {
                    return await _context.Contents.Where(c => c.ContentSchedule >= request.ViewVM.StartDate
                                                              && c.ContentSchedule <= request.ViewVM.EndDate)
                                                              .Include( c => c.ContentType)
                                                              .Include(c => c.Message)
                                                              .ToListAsync();
                }
                else if (request.ViewVM.ServiceId == 0 && request.ViewVM.OperatorId != 0)
                {
                    return await _context.Contents.Where(c => c.ContentSchedule >= request.ViewVM.StartDate
                                                              && c.ContentSchedule <= request.ViewVM.EndDate
                                                              && c.Message.OperatorId == request.ViewVM.OperatorId)
                                                              .Include(c => c.ContentType)
                                                              .Include(c => c.Message)
                                                              .ToListAsync();
                }
                else if (request.ViewVM.ServiceId != 0 && request.ViewVM.OperatorId == 0)
                {
                    return await _context.Contents.Where(c => c.ContentSchedule >= request.ViewVM.StartDate
                                                              && c.ContentSchedule <= request.ViewVM.EndDate
                                                              && c.Message.ServiceId == request.ViewVM.ServiceId)
                                                              .Include(c => c.ContentType)
                                                              .Include(c => c.Message)
                                                              //.Include(c => c.Message.Service)
                                                              .ToListAsync();
                }
                else
                {
                    return await _context.Contents.Where(c => c.ContentSchedule >= request.ViewVM.StartDate
                                                              && c.ContentSchedule <= request.ViewVM.EndDate
                                                              && c.Message.ServiceId == request.ViewVM.ServiceId
                                                              && c.Message.OperatorId == request.ViewVM.OperatorId)
                                                              .Include(c => c.ContentType)
                                                              .Include(c => c.Message)
                                                              //.Include(c => c.Message.Service)
                                                              .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
