using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.SMS.BLACKLIST.Command;
using Application.SMS.KEYWORD.Query;
using Application.SMS.SMSDN.Commands;
using Application.SMS.SUBSCRIPTION.Commands;
using Application.SMS.SUBSCRIPTION.Queries;
using Application.SMS.SUBSCRIPTION.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebCMS_Redi.Controllers
{
    [Authorize(Roles = "InternalManager,Administrator,CustomerService")]
    public class CustomerServiceController : BaseController
    {
        private readonly ILogger _logger = Serilog.Log.ForContext<ContentController>();
        private readonly ICurrentUserService _currentUserService;

        public CustomerServiceController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = new SubscriptionCsVM();
                vm = await Mediator.Send(new GetSubscriptionCsViewIndex { });
                return View(vm);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(SubscriptionCsVM vm)
        {
            try
            {
                //GetThe Subs VM View Here
                vm = await Mediator.Send(new GetSubscriptionCsView { Msisdn = vm.Msisdn, OperatorId = vm.OperatorId });
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<string> UnregMsisdn(string pMsisdn,int pServiceId, int pOperatorId)
        {
            try
            {
                //Get Sub
                var Sub = await Mediator.Send(new GetSubscription { Msisdn = pMsisdn, ServiceId = pServiceId, OperatorId = pOperatorId });
                //Get Main Keyword
                var Keyword = await Mediator.Send(new GetKeywordByServiceid { ServiceId = pServiceId });

                var UnregResult = await Mediator.Send(new UnregSubscription 
                                                    { rState = "UNREG BY CS", 
                                                      rSubscription = Sub, 
                                                      rUnreg_keyword = "UNREG " + Keyword.KeyWord });

                if (UnregResult.Succeeded)
                {
                    return "Unreg Msisdn: " + pMsisdn + " Succeeded";
                }
                else
                {
                    return UnregResult.Errors.FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<string> BlackListMsisdn(string pMsisdn, int pOperatorId)
        {
            try
            {
                var BlackListResult = await Mediator.Send(new InsertBlacklist
                {
                    Msisdn = pMsisdn,
                    OperatorId = pOperatorId
                });

                if (BlackListResult.Succeeded)
                {
                    return "Msisdn: " + pMsisdn + " Successfully BlackListed";
                }
                else
                {
                    return BlackListResult.Errors.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
