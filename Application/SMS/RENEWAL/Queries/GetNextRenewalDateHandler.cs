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

namespace Application.SMS.RENEWAL.Queries
{
    public class GetNextRenewalDateHandler : IRequestHandler<GetNextRenewalDate, DateTime>
    {
        private readonly IRediSmsDbContext _context;

        public GetNextRenewalDateHandler(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<DateTime> Handle(GetNextRenewalDate request, CancellationToken cancellationToken)
        {
            var RenewalConfigs = await _context.ServiceRenewalConfigurations.Where(r => r.ServiceId.Equals(request.rServiceid)
                                                                                    && r.OperatorId.Equals(request.rOperatorid))
                                                                                    .OrderBy(r => r.ScheduleOrder)
                                                                                    .ToListAsync();

            int NextRenewalDateDay = 1;
            DateTime rNextRenewDate = DateTime.Today.AddDays(NextRenewalDateDay);
            if (RenewalConfigs.Count.Equals(1) && RenewalConfigs.FirstOrDefault().IsSequence)
                NextRenewalDateDay = RenewalConfigs.FirstOrDefault().ScheduleSequence;
            else if (RenewalConfigs.Count >= 1 && !RenewalConfigs.FirstOrDefault().IsSequence)
            {
                var ScheduleDayOfWeeks = RenewalConfigs.Select(r => r.ScheduleDay).ToArray();
                Dictionary<DayOfWeek?, int> ScheduleDict = new Dictionary<DayOfWeek?, int>();
                foreach (var ScheduleDayOfWeek in ScheduleDayOfWeeks)
                {
                    if (ScheduleDayOfWeek != null)
                    {
                        int TempToNextRenewalDay = 1;
                        DateTime TempNextDay = new DateTime();
                        TempNextDay = DateTime.Today.AddDays(NextRenewalDateDay);
                        while ((int)ScheduleDayOfWeek != (int)TempNextDay.DayOfWeek)
                        {
                            TempToNextRenewalDay++;
                            TempNextDay = TempNextDay.AddDays(1);
                        }
                        ScheduleDict.Add(ScheduleDayOfWeek, TempToNextRenewalDay);
                    }
                }
                NextRenewalDateDay = ScheduleDict.Min(x => x.Value);
            }
            else
            {
                throw new NotFoundException(nameof(ServiceRenewalConfiguration), RenewalConfigs.FirstOrDefault());
            }

            return DateTime.Today.AddDays(NextRenewalDateDay);
        }
    }
}
