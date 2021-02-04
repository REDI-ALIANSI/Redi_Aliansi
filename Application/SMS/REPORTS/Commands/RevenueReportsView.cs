using Application.SMS.REPORTS.ViewModel;
using MediatR;

namespace Application.SMS.REPORTS.Commands
{
    public class RevenueReportsView : IRequest<RevenueReportVM>
    {
        public RevenueReportVM Vm { get; set; }
    }
}
