using System;
using System.Collections.Generic;
using Domain.Entities.SMS;
using Microsoft.EntityFrameworkCore;
using Application.Common.Model;
using MediatR;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Application.Common.Interfaces;

namespace Application.SMS.REPORTS.Commands
{
    public class GenerateSubscriptionsReportsHandler : IRequestHandler<GenerateSubscriptionsReports, Result>
    {
        private readonly IRediSmsDbContext _context;
        public GenerateSubscriptionsReportsHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GenerateSubscriptionsReports request, CancellationToken cancellationToken)
        {
            var RetResult = Result.Success();
            try
            {
                //get Services and Operators
                var Services = await _context.Services.Where(s => s.IsActive).ToListAsync();
                var Operators = await _context.Operators.ToListAsync();
                int New_member = 0;
                int Churn_member = 0;
                int Total_member = 0;

                //Start getting reports
                if (Services.Count > 0)
                {
                    foreach (var Service in Services)
                    {
                        foreach (var Operator in Operators)
                        {
                            //Get Total Member
                            Total_member = await _context.Subscriptions
                                            .Include(s => s.Service)
                                            .Where(s => s.ServiceId.Equals(Service.ServiceId)
                                                && s.OperatorId.Equals(Operator.OperatorId))
                                            .CountAsync();

                            if (Total_member > 0)
                            {
                                //Get  New Member on Historical table
                                New_member = await _context.SubscriptionHists
                                                .Include(s => s.Service)
                                                .Where(s => s.Subscription_Date >= request.GenDate.Date
                                                    && s.Subscription_Date < request.GenDate.AddDays(1).Date
                                                    && s.ServiceId.Equals(Service.ServiceId)
                                                    && s.OperatorId.Equals(Operator.OperatorId))
                                                .CountAsync();

                                //Add New Member from Live Table
                                New_member += await _context.Subscriptions
                                                .Include(s => s.Service)
                                                .Where(s => s.Subscription_Date >= request.GenDate.Date
                                                    && s.Subscription_Date < request.GenDate.AddDays(1).Date
                                                    && s.ServiceId.Equals(Service.ServiceId)
                                                    && s.OperatorId.Equals(Operator.OperatorId))
                                                .CountAsync();

                                //Get Unsubs Member
                                Churn_member = await _context.SubscriptionHists
                                                .Include(s => s.Service)
                                                .Where(s => s.Unsubscription_Date >= request.GenDate.Date
                                                    && s.Unsubscription_Date < request.GenDate.AddDays(1).Date
                                                    && s.ServiceId.Equals(Service.ServiceId)
                                                    && s.OperatorId.Equals(Operator.OperatorId))
                                                .CountAsync();

                                //Input to object and database
                                var SubscriptionReports = new SubscriptionReport()
                                {
                                    ServiceName = Service.Name,
                                    SDC = Service.Shortcode,
                                    Date = request.GenDate,
                                    New_Member = New_member,
                                    Churn_Member = Churn_member,
                                    Total_Member = Total_member,
                                    ServiceId = Service.ServiceId,
                                    OperatorId = Operator.OperatorId
                                };

                                //Save Object to Database
                                await _context.SubscriptionReports.AddAsync(SubscriptionReports);
                                await _context.SaveChangesAsync(cancellationToken);
                            }
                        }
                    }
                    return RetResult;
                }
                else
                {
                    var Error = new List<string>() { "ERROR: No Active Services Found!" };
                    RetResult = Result.Failure(Error);
                    return RetResult;
                }
            }
            catch(Exception ex)
            {
                var Error = new List<string>() { ex.Message };
                RetResult = Result.Failure(Error);
                return RetResult;
            }
        }
    }
}
