using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using Domain.Entities.SMS;
using Microsoft.EntityFrameworkCore;
using Application.Common.Model;
using MediatR;
using System.Linq;

namespace Application.SMS.REPORTS.Commands
{
    public class GenerateRevenueReportsHandler : IRequestHandler<GenerateRevenueReports, Result>
    {
        private readonly IRediSmsDbContext _context;
        public GenerateRevenueReportsHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GenerateRevenueReports request, CancellationToken cancellationToken)
        {
            var RetResult = Result.Success();
            try
            {
                //Get Services, Operators, and MessageType(s)
                var Services = await _context.Services.Where(s => s.IsActive).ToListAsync();
                var MessageTypes = new List<String>() { "PUSH","PULL","SUBS" };
                var Operators = await _context.Operators.ToListAsync();
                int Mt_sent = 0;
                int Mt_hits = 0;

                if (Services.Count > 0)
                {
                    foreach (var Service in Services)
                    {
                        foreach (var Operator in Operators)
                        {
                            foreach (var MessageType in MessageTypes)
                            {
                                List<string> Sids = new List<string>();
                                if (MessageType == "SUBS")
                                {
                                    Sids = await _context.SmsoutDs
                                                  .Where(s => s.ServiceId.Equals(Service.ServiceId)
                                                    && s.OperatorId.Equals(Operator.OperatorId)
                                                    && (s.Message.MessageType.Equals("ON") || s.Message.MessageType.Equals("OFF")))
                                                  .Include(s => s.Message)
                                                  .GroupBy(s => s.Message.SidBilling)
                                                  .Select(s => s.Key)
                                                  .ToListAsync();
                                }
                                else
                                {
                                    Sids = await _context.SmsoutDs
                                                  .Include(s => s.Message)
                                                  .Where(s => s.ServiceId.Equals(Service.ServiceId)
                                                    && s.OperatorId.Equals(Operator.OperatorId)
                                                    && s.Message.MessageType.Equals(MessageType))
                                                  .GroupBy(s => s.Message.SidBilling)
                                                  .Select(s => s.Key)
                                                  .ToListAsync();
                                }
                                
                                if (Sids.Count > 0)
                                {
                                    foreach (var Sid in Sids)
                                    {
                                        var ObjSid = await _context.Sids.Where(s => s.SidBilling == Sid).FirstOrDefaultAsync();
                                        if (MessageType == "PUSH" || MessageType == "PULL")
                                        {
                                            Mt_sent = await _context.SmsoutDs
                                                          .Include(s => s.Message)
                                                          .Where(s => s.ServiceId.Equals(Service.ServiceId)
                                                            && s.OperatorId.Equals(Operator.OperatorId)
                                                            && s.Message.MessageType.Equals(MessageType)
                                                            && s.Message.SidBilling.Equals(Sid))
                                                          .CountAsync();

                                            Mt_hits = await _context.SmsoutDs
                                                          .Include(s => s.Message)
                                                          .Include(s => s.SmsdnD)
                                                          .Where(s => s.ServiceId.Equals(Service.ServiceId)
                                                            && s.OperatorId.Equals(Operator.OperatorId)
                                                            && s.Message.MessageType.Equals(MessageType)
                                                            && s.Message.SidBilling.Equals(Sid)
                                                            && s.SmsdnD.Status.Equals("Delivered"))
                                                          .CountAsync();
                                        }
                                        else
                                        {
                                            Mt_sent = await _context.SmsoutDs
                                                          .Include(s => s.Message)
                                                          .Where(s => s.ServiceId.Equals(Service.ServiceId)
                                                            && s.OperatorId.Equals(Operator.OperatorId)
                                                            && (s.Message.MessageType.Equals("ON")
                                                                || s.Message.MessageType.Equals("OFF")
                                                                || s.Message.MessageType.Equals(MessageType))
                                                            && s.Message.SidBilling.Equals(Sid))
                                                          .CountAsync();

                                            Mt_hits = await _context.SmsoutDs
                                                          .Include(s => s.Message)
                                                          .Include(s => s.SmsdnD)
                                                          .Where(s => s.ServiceId.Equals(Service.ServiceId)
                                                            && s.OperatorId.Equals(Operator.OperatorId)
                                                            && (s.Message.MessageType.Equals("ON")
                                                                || s.Message.MessageType.Equals("OFF")
                                                                || s.Message.MessageType.Equals(MessageType))
                                                            && s.Message.SidBilling.Equals(Sid)
                                                            && s.SmsdnD.Status.Equals("Delivered"))
                                                          .CountAsync();
                                        }
                                        //Put the Revenue report object
                                        var RevenueReport = new RevenueReport
                                        {
                                            Date = request.GenDate,
                                            Mt_Sent = Mt_sent,
                                            Mt_Hits = Mt_hits,
                                            SDC = Service.Shortcode,
                                            Sid = Sid,
                                            Price = ObjSid.Price,
                                            Mt_Type = MessageType,
                                            Revenue = Mt_hits * ObjSid.Price,
                                            ServiceId = Service.ServiceId,
                                            OperatorId = Operator.OperatorId
                                        };
                                        //Save Object to Database
                                        await _context.RevenueReports.AddAsync(RevenueReport);
                                        await _context.SaveChangesAsync(cancellationToken);
                                    }
                                }
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
            catch (Exception ex)
            {
                var Error = new List<string>() { ex.Message };
                RetResult = Result.Failure(Error);
                return RetResult;
            }
        }
    }
}
