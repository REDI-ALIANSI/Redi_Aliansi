using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Application.Common.Exceptions;
using Application.SMS.RENEWAL.ViewModel;

namespace Application.SMS.RENEWAL.Commands
{
    public class DailyPrepRenewalHandler : IRequestHandler<DailyPrepRenewal,List<RenewalVM>>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;

        public DailyPrepRenewalHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<List<RenewalVM>> Handle(DailyPrepRenewal request, CancellationToken cancellationToken)
        {
            try
            {
                var RenewalVMList = new List<RenewalVM>();
                var ListService = await _context.Services.Where(s => s.IsActive).ToListAsync();
                if (ListService.Count > 0)
                {
                    foreach (var service in ListService)
                    {
                        var renewVM = new RenewalVM
                        {
                            ServiceName = service.Name
                        };
                        var ListServiceRenewalConfig = await _context.ServiceRenewalConfigurations.Where(c => c.ServiceId == service.ServiceId
                                                                                                        && c.IsActive && (c.ScheduleDay == DateTime.Today.DayOfWeek || c.IsSequence))
                                                                                                        .Include(r => r.Service)
                                                                                                        .AsNoTracking()
                                                                                                        .ToListAsync();
                        int RenewalConfigCount = ListServiceRenewalConfig.Count;
                        if (RenewalConfigCount > 0)
                        {
                            var OperatorList = ListServiceRenewalConfig.GroupBy(sr => sr.OperatorId).ToList();

                            foreach (var Operatorid in OperatorList)
                            {
                                var ListServicebyOperator = ListServiceRenewalConfig.Where(ls => ls.OperatorId == Operatorid.Key).ToList();
                                int CountServiceRenewalConfig = await _context.ServiceRenewalConfigurations.Where(sr => sr.ServiceId.Equals(service.ServiceId)
                                                                                                        && sr.OperatorId.Equals(Operatorid.Key)).CountAsync();
                                foreach (var RenewalConfig in ListServicebyOperator)
                                {
                                    //Build function to process Renewal Config
                                    await _mediator.Send(new ProcessRenewalConfig
                                    { renewalConfig = RenewalConfig, rRenewalTime = request.RenewalTime, rRenewalConfigCount = CountServiceRenewalConfig, QueueAuth = request.QueueAuth }, cancellationToken);
                                }
                            }
                        }
                        RenewalVMList.Add(renewVM);
                    }
                }
                else
                {
                    var renewVM = new RenewalVM
                    {
                        ServiceName = "SERVICE NULL",
                        MessagesGenerated = 0,
                        GenerateSpan = new TimeSpan(0)
                    };
                    RenewalVMList.Add(renewVM);
                }

                return RenewalVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
