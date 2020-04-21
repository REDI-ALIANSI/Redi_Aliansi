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
                        var ListServiceRenewalConfig = await _context.ServiceRenewalConfigurations.Where(c => c.ServiceId.Equals(service.ServiceId) && c.IsActive)
                                                                                                    .Include(c => c.Message)
                                                                                                    .ToListAsync();
                        int RenewalConfigCount = ListServiceRenewalConfig.Count;
                        foreach (var RenewalConfig in ListServiceRenewalConfig)
                        {
                            //Build function to process Renewal Config
                            await _mediator.Send(new ProcessRenewalConfig
                            { renewalConfig = RenewalConfig, rRenewalTime = request.RenewalTime, rRenewalConfigCount  = RenewalConfigCount},cancellationToken);
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
