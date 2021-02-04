using Application.Common.Interfaces;
using Application.SMS.CONTENT.Queries;
using Application.SMS.CONTENT.ViewModel;
using Application.SMS.OPERATOR.Queries;
using Application.SMS.SERVICE.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.CONTENT.Command
{
    public class ViewIndexContentHandler : IRequestHandler<ViewIndexContent, ContentViewVM>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;
        public ViewIndexContentHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<ContentViewVM> Handle(ViewIndexContent request, CancellationToken cancellationToken)
        {
            try
            {
                //get Operator and Service to seect list Item
                var ListOperators = await _mediator.Send(new GetOperators { });
                ListOperators.Add(new Domain.Entities.Operator { OperatorId = 0, OperatorName = "ALL" });
                var SelectListOperator = new SelectList(ListOperators, "OperatorId", "OperatorName");
                SelectListOperator.Where(o => o.Value.Equals("0")).FirstOrDefault().Selected = true;
                var ListServices = await _mediator.Send(new GetServices { });
                ListServices.Add(new Domain.Entities.SMS.Service { ServiceId = 0, Name = "ALL" });
                var selectListService = new SelectList(ListServices, "ServiceId", "Name");
                selectListService.Where(o => o.Value.Equals("0")).FirstOrDefault().Selected = true;

                var vm = new ContentViewVM()
                {
                    StartDate = request.contentViewVM.StartDate,
                    EndDate = request.contentViewVM.EndDate,
                    ServiceId = request.contentViewVM.ServiceId,
                    OperatorId = request.contentViewVM.OperatorId,
                    ListVMService = selectListService,
                    ListVMOperator = SelectListOperator
                };

                //Get Contents here
                vm.Contents = await _mediator.Send(new GetContentView { ViewVM = vm });

                return vm;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
