using System.Collections.Generic;
using Application.SMS.REPORTS.ViewModel;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.REPORTS.Queries
{
    public class GetCampaignReports : IRequest<List<CampaignReport>>
    {
        public CampaignReportVM Vm { get; set; }
    }
}
