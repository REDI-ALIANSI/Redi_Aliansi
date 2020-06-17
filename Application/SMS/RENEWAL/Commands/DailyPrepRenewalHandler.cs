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

namespace Application.SMS.RENEWAL.Commands
{
    public class DailyPrepRenewalHandler : IRequestHandler<DailyPrepRenewal>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;

        public DailyPrepRenewalHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DailyPrepRenewal request, CancellationToken cancellationToken)
        {
            try
            {
                var ListService = await _context.Services.ToListAsync();
                if (ListService.Count > 0)
                {
                    foreach (var service in ListService)
                    {
                        var ListServiceRenewalConfig = await _context.ServiceRenewalConfigurations.Where(c => c.ServiceId == service.ServiceId 
                                                                                                        && c.IsActive && (c.ScheduleDay == DateTime.Today.DayOfWeek || c.IsSequence))
                                                                                                    .Include(c => c.Message)
                                                                                                    .ToListAsync();
                        int RenewalConfigCount = ListServiceRenewalConfig.Count;
                        if (RenewalConfigCount > 0)
                        {
                            foreach (var RenewalConfig in ListServiceRenewalConfig)
                            {
                                //Build function to process Renewal Config
                                await _mediator.Send(new ProcessRenewalConfig
                                { renewalConfig = RenewalConfig, rRenewalTime = request.RenewalTime, rRenewalConfigCount = RenewalConfigCount, QueueAuth = request.QueueAuth }, cancellationToken);
                            }
                        }
                    }
                }
                else
                {
                    throw new NotFoundException(nameof(ServiceRenewalConfiguration), ListService);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
