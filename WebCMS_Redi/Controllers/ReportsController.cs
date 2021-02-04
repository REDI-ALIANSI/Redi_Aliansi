using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.SMS.REPORTS.Commands;
using Application.SMS.REPORTS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebCMS_Redi.Controllers
{
    [Authorize(Roles = "InternalManager,Administrator,ReportAdm,TelcoAdm")]
    public class ReportsController : BaseController
    {
        private readonly ILogger _logger = Serilog.Log.ForContext<ContentController>();
        private readonly ICurrentUserService _currentUserService;

        public ReportsController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> RevenueReports()
        {
            try
            {
                var vm = new RevenueReportVM()
                {
                    StartDate = DateTime.Today.AddDays(-1),
                    EndDate = DateTime.Today,
                    ServiceId = 0,
                    OperatorId = 0,
                    Sdc = 99599,
                    Type = 0
                };

                vm = await Mediator.Send(new RevenueReportsView { Vm = vm });

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> RevenueReports(RevenueReportVM vm)
        {
            try
            {
                vm = await Mediator.Send(new RevenueReportsView { Vm = vm });

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        public async Task<IActionResult> SubscriptionReports()
        {
            try
            {
                var vm = new SubscriptionsReportVM()
                {
                    StartDate = DateTime.Today.AddDays(-1),
                    EndDate = DateTime.Today,
                    ServiceId = 0,
                    OperatorId = 0,
                    Sdc = 99599
                };

                vm = await Mediator.Send(new SubscriptionReportsView { Vm = vm });

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubscriptionReports(SubscriptionsReportVM vm)
        {
            try
            {
                vm = await Mediator.Send(new SubscriptionReportsView { Vm = vm });

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        public async Task<IActionResult> CampaignReports()
        {
            try
            {
                var vm = new CampaignReportVM()
                {
                    StartDate = DateTime.Today.AddDays(-1),
                    EndDate = DateTime.Today,
                    ServiceId = 0,
                    OperatorId = 0
                };

                vm = await Mediator.Send(new CampaignReportsView { Vm = vm });

                return View(vm);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CampaignReports(CampaignReportVM vm)
        {
            try
            {
                vm = await Mediator.Send(new CampaignReportsView { Vm = vm });

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }
    }
}
