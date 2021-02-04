using System.Collections.Generic;
using Application.SMS.REPORTS.ViewModel;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.REPORTS.Queries
{
    public class GetRevenueReports : IRequest<List<RevenueReport>>
    {
        public RevenueReportVM Vm { get; set; }
    }
}
