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
    class GetCampaignReportsHandler : IRequestHandler<GetCampaignReports, List<CampaignReport>>
    {
        private readonly IRediSmsDbContext _context;

        public GetCampaignReportsHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<List<CampaignReport>> Handle(GetCampaignReports request, CancellationToken cancellationToken)
        {
            try
            {
                var CampReports = await _context.CampaignReports
                                            .Include(r => r.ServiceCampaign)
                                            .Include(r => r.ServiceCampaign.Service)
                                            .Include(r => r.ServiceCampaign.Operator)
                                            .Where(r => r.Date >= request.Vm.StartDate
                                            && r.Date <= request.Vm.EndDate 
                                            && r.ServiceCampaign.CampaignKeyword == request.Vm.CampaignKeyword)
                                            .OrderBy(r => r.Date)
                                            .ToListAsync();

                //Filter Reports
                if (!request.Vm.OperatorId.Equals(0))
                    CampReports = CampReports.Where(r => r.ServiceCampaign.OperatorId == request.Vm.OperatorId).ToList();

                if (!request.Vm.ServiceId.Equals(0))
                    CampReports = CampReports.Where(r => r.ServiceCampaign.ServiceId == request.Vm.ServiceId).ToList();

                return CampReports;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
