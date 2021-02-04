using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.SMS.BLACKLIST.Query;
using Application.SMS.OPERATOR.Queries;
using Application.SMS.SUBSCRIPTION.ViewModel;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Application.SMS.SUBSCRIPTION.Queries
{
    public class GetSubscriptionCsView : IRequest<SubscriptionCsVM>
    {
        public string Msisdn { get; set; }
        public int OperatorId { get; set; }
    }

    public class GetSubscriptionCsViewHandler : IRequestHandler<GetSubscriptionCsView, SubscriptionCsVM>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;

        public GetSubscriptionCsViewHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<SubscriptionCsVM> Handle(GetSubscriptionCsView request, CancellationToken cancellationToken)
        {
            try
            {
                //Get Operator List
                var ListOperators = await _mediator.Send(new GetOperators { });
                var SelectListOperator = new SelectList(ListOperators, "OperatorId", "OperatorName");
                //Declare VM
                var vm = new SubscriptionCsVM()
                { Msisdn = request.Msisdn, OperatorId = request.OperatorId, ListVMOperator = SelectListOperator };
                //Check BlackLis
                vm.IsBlacklisted = await _mediator.Send(new IsBlacklist { Msisdn = request.Msisdn, OperatorId = request.OperatorId });

                //Declare Subscriptions List detail
                var ListSubsDet = new List<SubscriptionsCsVMList>();
                
                //GetSubs from table Subscriptions
                var Subs = await _context.Subscriptions.Where(s => s.Msisdn == request.Msisdn && s.OperatorId == request.OperatorId).ToListAsync();
                
                if(Subs != null)
                {
                    //Get Subs on each subs list
                    foreach (var Sub in Subs)
                    {
                        var vmSub = new SubscriptionsCsVMList()
                        {
                            ServiceId = Sub.ServiceId,
                            Subscrition_Date = Sub.Subscription_Date,
                            Unsubscription_Date = null,
                            Iservice = await _context.Services.Where(s => s.ServiceId.Equals(Sub.ServiceId)).FirstOrDefaultAsync(),
                            TotalCharged = Sub.Total_Revenue
                        };
                        vm.Total_Charged += Sub.Total_Revenue;
                        ListSubsDet.Add(vmSub);
                    }
                }

                //Declare SubscriptionHists List
                var SubHists = new List<SubscriptionHist>();
                //Get Subs from table SubscriptionHists
                SubHists = await _context.SubscriptionHists.Where(s => s.Msisdn == request.Msisdn && s.OperatorId == request.OperatorId).ToListAsync();
                if (SubHists != null)
                {
                    //get Subs on each SubHists
                    foreach (var SubHist in SubHists)
                    {
                        var vmSub = new SubscriptionsCsVMList()
                        {
                            ServiceId = SubHist.ServiceId,
                            Subscrition_Date = SubHist.Subscription_Date,
                            Unsubscription_Date = SubHist.Unsubscription_Date,
                            Iservice = await _context.Services.Where(s => s.ServiceId.Equals(SubHist.ServiceId)).FirstOrDefaultAsync(),
                            TotalCharged = SubHist.Total_Revenue
                        };
                        vm.Total_Charged += SubHist.Total_Revenue;
                        ListSubsDet.Add(vmSub);
                    }
                }

                vm.iOperator = await _context.Operators.Where(o => o.OperatorId == request.OperatorId).FirstOrDefaultAsync();

                vm.Subscriptions = ListSubsDet;
                return vm;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
