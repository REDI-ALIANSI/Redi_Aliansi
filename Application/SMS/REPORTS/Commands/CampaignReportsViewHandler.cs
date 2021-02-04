using Application.Common.Interfaces;
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
    public class CampaignReportsViewHandler : IRequestHandler<CampaignReportsView, CampaignReportVM>
    {
        private readonly IMediator _mediator;
        private readonly IRediSmsDbContext _context;

        public CampaignReportsViewHandler(IMediator mediator,IRediSmsDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }
        public async Task<CampaignReportVM> Handle(CampaignReportsView request, CancellationToken cancellationToken)
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
                //Get List Campaign Keywords to select list rendering object
                var ListKeywords = await _mediator.Send(new GetCampaignReportKeywords { });
                var selectListKeywords = new SelectList(ListKeywords, "Key", "Value");

                var vm = new CampaignReportVM()
                {
                    StartDate = request.Vm.StartDate,
                    EndDate = request.Vm.EndDate,
                    ServiceId = request.Vm.ServiceId,
                    OperatorId = request.Vm.OperatorId,
                    CampaignKeyword = request.Vm.CampaignKeyword,
                    ListVMService = selectListService,
                    ListKeywords = selectListKeywords,
                    ListVMOperator = SelectListOperator
                };

                if (ListKeywords.Count > 0)
                    vm.CampaignKeyword = ListKeywords.FirstOrDefault().Value;

                vm.Campaignreports = await _mediator.Send(new GetCampaignReports { Vm = vm });

                return vm;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
