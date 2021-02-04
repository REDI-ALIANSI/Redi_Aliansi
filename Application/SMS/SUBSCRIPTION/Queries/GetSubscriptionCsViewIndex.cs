using Application.Common.Interfaces;
using Application.SMS.OPERATOR.Queries;
using Application.SMS.SUBSCRIPTION.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.SUBSCRIPTION.Queries
{
    public class GetSubscriptionCsViewIndex : IRequest<SubscriptionCsVM>
    {

    }

    public class GetSubscriptionCsViewIndexHandler : IRequestHandler<GetSubscriptionCsViewIndex, SubscriptionCsVM>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;

        public GetSubscriptionCsViewIndexHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<SubscriptionCsVM> Handle(GetSubscriptionCsViewIndex request, CancellationToken cancellationToken)
        {
            //Get Operator List
            var ListOperators = await _mediator.Send(new GetOperators { });
            var SelectListOperator = new SelectList(ListOperators, "OperatorId", "OperatorName");

            return new SubscriptionCsVM() { ListVMOperator = SelectListOperator };
        }
    }
}
