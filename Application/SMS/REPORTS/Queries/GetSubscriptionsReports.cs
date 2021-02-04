using System;
using System.Collections.Generic;
using Application.SMS.REPORTS.ViewModel;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.REPORTS.Queries
{
    public class GetSubscriptionsReports : IRequest<List<SubscriptionReport>>
    {
        public SubscriptionsReportVM Vm { get; set; }
    }
}
