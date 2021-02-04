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
    public class GetSubscriptionsReportsHandler : IRequestHandler<GetSubscriptionsReports, List<SubscriptionReport>>
    {
        private readonly IRediSmsDbContext _context;

        public GetSubscriptionsReportsHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<List<SubscriptionReport>> Handle(GetSubscriptionsReports request, CancellationToken cancellationToken)
        {
            try
            {
                var SubsReports = await _context.SubscriptionReports.Where(s => s.Date >= request.Vm.StartDate
                                                    && s.Date <= request.Vm.EndDate)
                                                    .Include(s => s.Service)
                                                    .Include(s => s.Operator)
                                                    .OrderBy(s => s.Date)
                                                    .ToListAsync();

                //Filter Reports
                if (!request.Vm.OperatorId.Equals(0))
                    SubsReports = SubsReports.Where(r => r.OperatorId == request.Vm.OperatorId).ToList();

                if (!request.Vm.ServiceId.Equals(0))
                    SubsReports = SubsReports.Where(r => r.ServiceId == request.Vm.ServiceId).ToList();

                if (!request.Vm.Sdc.Equals(0))
                    SubsReports = SubsReports.Where(r => r.SDC == request.Vm.Sdc).ToList();

                return SubsReports;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
