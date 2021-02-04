using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using Domain.Entities.SMS;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Linq;

namespace Application.SMS.REPORTS.Queries
{
    public class GetRevenueReportsHandler : IRequestHandler<GetRevenueReports, List<RevenueReport>>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;

        public GetRevenueReportsHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _mediator = mediator;
            _context = context;
        }
        public async Task<List<RevenueReport>> Handle(GetRevenueReports request, CancellationToken cancellationToken)
        {
            try
            {
                var RevReports = await _context.RevenueReports.Where(r => r.Date >= request.Vm.StartDate
                                            && r.Date <= request.Vm.EndDate)
                                            .Include(r => r.Service)
                                            .Include(r => r.Operator)
                                            .OrderBy(r => r.Date)
                                            .ToListAsync();

                var MessageReportTypes = await _mediator.Send(new GetReportMessageType { });

                //Filter Reports
                if (!request.Vm.OperatorId.Equals(0))
                    RevReports = RevReports.Where(r => r.OperatorId == request.Vm.OperatorId).ToList();

                if (!request.Vm.ServiceId.Equals(0))
                    RevReports = RevReports.Where(r => r.ServiceId == request.Vm.ServiceId).ToList();

                if (!request.Vm.Sdc.Equals(0))
                    RevReports = RevReports.Where(r => r.SDC == request.Vm.Sdc).ToList();

                if (!request.Vm.Type.Equals(0))
                {
                    var MessageType = MessageReportTypes.Where(m => m.Key.Equals(request.Vm.Type)).Select(m => m.Value).FirstOrDefault();
                    RevReports = RevReports.Where(r => r.Mt_Type == MessageType).ToList();
                }

                return RevReports;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
