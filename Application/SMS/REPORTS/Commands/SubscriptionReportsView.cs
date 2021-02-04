using Application.SMS.REPORTS.ViewModel;
using MediatR;

namespace Application.SMS.REPORTS.Commands
{
    public class SubscriptionReportsView : IRequest<SubscriptionsReportVM>
    {
        public SubscriptionsReportVM Vm { get; set; }
    }
}
