using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.SMS.SUBSCRIPTION.Commands
{
    public class InsertSubscriptionHandler : IRequestHandler<InsertSubscription, Subscription>
    {
        private readonly IRediSmsDbContext _context;

        public InsertSubscriptionHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<Subscription> Handle(InsertSubscription request, CancellationToken cancellationToken)
        {
            try
            {
                //Get Next renewDate
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

                    NextRenewalDateDay = ScheduleDict.Min().Value;
                }
                else
                {
                    throw new NotFoundException(nameof(ServiceRenewalConfiguration), RenewalConfigs.FirstOrDefault());
                }

                rNextRenewDate = DateTime.Today.AddDays(NextRenewalDateDay);
                var NewSubs = new Subscription()
                { 
                    Msisdn = request.rMsisdn,
                    Reg_Keyword = request.rMoMessage,
                    Subscription_Date = DateTime.Now,
                    Next_Renew_Time = rNextRenewDate,
                    Total_Revenue = 0,
                    Mt_Sent = 0,
                    Mt_Success = 0,
                    ServiceId = request.rServiceid,
                    OperatorId = request.rOperatorid,
                    //Shortcode = request.rShortcode
                };

                await _context.Subscriptions.AddAsync(NewSubs);
                await _context.SaveChangesAsync(cancellationToken);

                return NewSubs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
