using Application.Common.Interfaces;
using Application.SMS.MESSAGE.Queries;
using Application.SMS.OPERATOR.Queries;
using Application.SMS.REPORTS.Queries;
using Application.SMS.REPORTS.ViewModel;
using Application.SMS.SERVICE.Queries;
using Application.SMS.SHORTCODE.Queries;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.REPORTS.Commands
{
    public class SubscriptionReportsViewHandler : IRequestHandler<SubscriptionReportsView, SubscriptionsReportVM>
    {
        private readonly IMediator _mediator;

        public SubscriptionReportsViewHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<SubscriptionsReportVM> Handle(SubscriptionReportsView request, CancellationToken cancellationToken)
        {
            try
            {
                //get Operator to select list rendering object
                var ListOperators = await _mediator.Send(new GetOperators { });
                ListOperators.Add(new Domain.Entities.Operator { OperatorId = 0, OperatorName = "ALL" });
                var SelectListOperator = new SelectList(ListOperators, "OperatorId", "OperatorName");
                SelectListOperator.Where(o => o.Value.Equals("0")).FirstOrDefault().Selected = true;
                //Get Services to select list rendering object
                var ListServices = await _mediator.Send(new GetServices { });
                ListServices.Add(new Domain.Entities.SMS.Service { ServiceId = 0, Name = "ALL" });
                var selectListService = new SelectList(ListServices, "ServiceId", "Name");
                selectListService.Where(o => o.Value.Equals("0")).FirstOrDefault().Selected = true;
                //Get Sdc List to select list rendering object
                var SdcList = await _mediator.Send(new GetSdcList { });
                var selectListSdc = new SelectList(SdcList, "Shortcode", "Description");

                var vm = new SubscriptionsReportVM()
                {
                    StartDate = request.Vm.StartDate,
                    EndDate = request.Vm.EndDate,
                    ServiceId = request.Vm.ServiceId,
                    OperatorId = request.Vm.OperatorId,
                    Sdc = request.Vm.Sdc,
                    ListVMService = selectListService,
                    ListVMOperator = SelectListOperator,
                    ListSdc = selectListSdc,
                };

                vm.Subscriptionreports = await _mediator.Send(new GetSubscriptionsReports { Vm = vm });

                return vm;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
