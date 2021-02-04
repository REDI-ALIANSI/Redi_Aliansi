using Application.SMS.OPERATOR.Queries;
using Application.SMS.REPORTS.Queries;
using Application.SMS.REPORTS.ViewModel;
using Application.SMS.SERVICE.Queries;
using Application.SMS.SHORTCODE.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.REPORTS.Commands
{
    public class RevenueReportsViewHandler : IRequestHandler<RevenueReportsView, RevenueReportVM>
    {
        private readonly IMediator _mediator;

        public RevenueReportsViewHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<RevenueReportVM> Handle(RevenueReportsView request, CancellationToken cancellationToken)
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
                //Get Message Type to select list rendering object
                var ListMessageType = await _mediator.Send(new GetReportMessageType { });
                var SelectListMessageType = new SelectList(ListMessageType,"Key", "Value");
                //Get Sdc List to select list rendering object
                var SdcList = await _mediator.Send(new GetSdcList { });
                var selectListSdc = new SelectList(SdcList, "Shortcode", "Description");

                var vm = new RevenueReportVM()
                {
                    StartDate = request.Vm.StartDate,
                    EndDate = request.Vm.EndDate,
                    ServiceId = request.Vm.ServiceId,
                    OperatorId = request.Vm.OperatorId,
                    Type = request.Vm.Type,
                    Sdc = request.Vm.Sdc,
                    ListVMService = selectListService,
                    ListVMOperator = SelectListOperator,
                    ListSdc = selectListSdc,
                    ListType = SelectListMessageType
                };

                vm.Revenuereports = await _mediator.Send(new GetRevenueReports { Vm = request.Vm });

                return vm;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
