using Application.SMS.REPORTS.ViewModel;
using MediatR;

namespace Application.SMS.REPORTS.Commands
{
    public class CampaignReportsView : IRequest<CampaignReportVM>
    {
        public CampaignReportVM Vm { get; set; }
    }
}
