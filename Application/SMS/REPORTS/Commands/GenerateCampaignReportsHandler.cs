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
    public class GenerateCampaignReportsHandler : IRequestHandler<GenerateCampaignReports, Result>
    {
        private readonly IRediSmsDbContext _context;
        public GenerateCampaignReportsHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GenerateCampaignReports request, CancellationToken cancellationToken)
        {
            var RetResult = Result.Success();
            try
            {
                //Get Campaign Services and declare variables
                var serviceCampaigns = await _context.ServiceCampaigns.Where(s => s.IsActive && s.ExpiredDate >= DateTime.Today).ToListAsync();
                int New_member = 0;
                int Churn_member = 0;
                int Total_member = 0;
                int Mt_sent = 0;
                int Mt_Hits = 0;

                if (serviceCampaigns.Count > 0)
                {
                    foreach (var serviceCampaign in serviceCampaigns)
                    {
                        if (serviceCampaign.IsCallBackRequired)
                        {
                            //Get New_member MSISDNs
                            var new_subs = new List<string>();

                            var new_in_subs = await _context.SmsinDs.Where(s => s.DateCreated >= request.GenDate.Date
                                                    && s.DateCreated < request.GenDate.AddDays(1).Date
                                                    && s.Mo_Message.ToUpper().StartsWith("REG " + serviceCampaign.CampaignKeyword.ToUpper())
                                                    && s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                    && s.OperatorId.Equals(serviceCampaign.OperatorId))
                                              .Select(s => s.Msisdn)
                                              .ToListAsync();

                            //added MSISDN List on table SMSIND
                            new_subs.AddRange(new_in_subs);

                            //Get New member from Hist
                            New_member = await _context.SmsinDs.Where(s => s.DateCreated >= request.GenDate.Date
                                                    && s.DateCreated < request.GenDate.AddDays(1).Date
                                                    && s.Mo_Message.ToUpper().StartsWith("REG " + serviceCampaign.CampaignKeyword.ToUpper())
                                                    && s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                    && s.OperatorId.Equals(serviceCampaign.OperatorId))
                                              .CountAsync();

                            Churn_member = await _context.SubscriptionHists
                                                .Where(s => s.Unsubscription_Date >= request.GenDate.Date
                                                    && s.Unsubscription_Date < request.GenDate.AddDays(1).Date
                                                    && s.Unreg_keyword.ToUpper().StartsWith("UNREG " + serviceCampaign.CampaignKeyword.ToUpper())
                                                    && s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                    && s.OperatorId.Equals(serviceCampaign.OperatorId))
                                                .CountAsync();

                            //Get Total Member
                            Total_member = await _context.Subscriptions
                                            .Include(s => s.Service)
                                            .Where(s => s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                && s.OperatorId.Equals(serviceCampaign.OperatorId)
                                                && s.Reg_Keyword.ToUpper().StartsWith("REG " + serviceCampaign.CampaignKeyword.ToUpper()))
                                            .CountAsync();

                            //Get Mt_Hits
                            Mt_Hits = await _context.SmsoutDs
                                                          .Include(s => s.Message)
                                                          .Include(s => s.SmsdnD)
                                                          .Include(s => s.Message.Sid)
                                                          .Where(s => s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                            && s.OperatorId.Equals(serviceCampaign.OperatorId)
                                                            && s.Message.MessageType.Equals("ON")
                                                            && s.Message.Sid.Price > 0
                                                            && s.SmsdnD.Status.Equals("Delivered")
                                                            && new_in_subs.Contains(s.Msisdn))
                                                          .CountAsync();
                            //Get MT_sent
                            Mt_sent = await _context.SmsoutDs
                                                          .Include(s => s.Message)
                                                          .Include(s => s.SmsdnD)
                                                          .Include(s => s.Message.Sid)
                                                          .Where(s => s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                            && s.OperatorId.Equals(serviceCampaign.OperatorId)
                                                            && s.Message.MessageType.Equals("ON")
                                                            && s.Message.Sid.Price > 0
                                                            && new_in_subs.Contains(s.Msisdn))
                                                          .CountAsync();
                        }
                        else
                        {
                            //Get New_member MSISDN
                            var new_subs = new List<string>();

                            var new_in_subs = await _context.SmsinDs.Where(s => s.DateCreated >= request.GenDate.Date
                                                    && s.DateCreated < request.GenDate.AddDays(1).Date
                                                    && s.Mo_Message.ToUpper().Equals("REG " + serviceCampaign.CampaignKeyword.ToUpper())
                                                    && s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                    && s.OperatorId.Equals(serviceCampaign.OperatorId))
                                              .Select(s => s.Msisdn)
                                              .ToListAsync();

                            //added MSISDN List on table SMSIND
                            new_subs.AddRange(new_in_subs);

                            //Get New member from Hist
                            New_member = await _context.SmsinDs.Where(s => s.DateCreated >= request.GenDate.Date
                                                    && s.DateCreated < request.GenDate.AddDays(1).Date
                                                    && s.Mo_Message.ToUpper().Equals("REG " + serviceCampaign.CampaignKeyword.ToUpper())
                                                    && s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                    && s.OperatorId.Equals(serviceCampaign.OperatorId))
                                              .CountAsync();

                            Churn_member = await _context.SubscriptionHists
                                                .Where(s => s.Unsubscription_Date >= request.GenDate.Date
                                                    && s.Unsubscription_Date < request.GenDate.AddDays(1).Date
                                                    && s.Unreg_keyword.ToUpper().Equals("UNREG " + serviceCampaign.CampaignKeyword.ToUpper())
                                                    && s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                    && s.OperatorId.Equals(serviceCampaign.OperatorId))
                                                .CountAsync();

                            //Get Total Member
                            Total_member = await _context.Subscriptions
                                            .Include(s => s.Service)
                                            .Where(s => s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                && s.OperatorId.Equals(serviceCampaign.OperatorId)
                                                && s.Reg_Keyword.ToUpper().Equals("REG " + serviceCampaign.CampaignKeyword.ToUpper()))
                                            .CountAsync();

                            //Get Mt_Hits
                            Mt_Hits = await _context.SmsoutDs
                                                          .Include(s => s.Message)
                                                          .Include(s => s.SmsdnD)
                                                          .Include(s => s.Message.Sid)
                                                          .Where(s => s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                            && s.OperatorId.Equals(serviceCampaign.OperatorId)
                                                            && s.Message.MessageType.Equals("ON")
                                                            && s.Message.Sid.Price > 0
                                                            && s.SmsdnD.Status.Equals("Delivered")
                                                            && new_in_subs.Contains(s.Msisdn))
                                                          .CountAsync();
                            //Get MT_sent
                            Mt_sent = await _context.SmsoutDs
                                                          .Include(s => s.Message)
                                                          .Include(s => s.SmsdnD)
                                                          .Include(s => s.Message.Sid)
                                                          .Where(s => s.ServiceId.Equals(serviceCampaign.ServiceId)
                                                            && s.OperatorId.Equals(serviceCampaign.OperatorId)
                                                            && s.Message.MessageType.Equals("ON")
                                                            && s.Message.Sid.Price > 0
                                                            && new_in_subs.Contains(s.Msisdn))
                                                          .CountAsync();
                        }

                        var Message = await _context.Messages
                                                .Include(m => m.Sid)
                                                .Where(m => m.MessageType.Equals("ON")
                                                && m.ServiceId.Equals(serviceCampaign.ServiceId)
                                                && m.OperatorId.Equals(serviceCampaign.OperatorId)
                                                && m.Sid.Price > 0).FirstOrDefaultAsync();

                        var MtPrice = Message.Sid.Price;

                        // Save Object to Database
                        var CampaignReport = new CampaignReport()
                        {
                            Date = request.GenDate,
                            New_Member = New_member,
                            Churn_Member = Churn_member,
                            Total_Member = Total_member,
                            Mt_Hits = Mt_Hits,
                            Revenue = Mt_Hits * MtPrice,
                            ServiceCampaignId = serviceCampaign.CampaignId
                        };
                        //Save Object to Database
                        await _context.CampaignReports.AddAsync(CampaignReport);
                        await _context.SaveChangesAsync(cancellationToken);
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
            catch (Exception ex)
            {
                var Error = new List<string>() { ex.Message };
                RetResult = Result.Failure(Error);
                return RetResult;
            }
        }
    }
}
