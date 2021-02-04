using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.REPORTS.Queries
{
    public class GetCampaignReportKeywords : IRequest<Dictionary<int,string>>
    {

    }

    public class GetCampaignReportKeywordsHandler : IRequestHandler<GetCampaignReportKeywords, Dictionary<int, string>>
    {
        private readonly IRediSmsDbContext _context;
        public GetCampaignReportKeywordsHandler (IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<int, string>> Handle(GetCampaignReportKeywords request, CancellationToken cancellationToken)
        {
            try
            {
                var countCampaignKeywords = await _context.ServiceCampaigns.CountAsync();
                Dictionary<int, string> DictCampaignKeywords = new Dictionary<int, string>();
                if (countCampaignKeywords > 0)
                {
                    var CampaignKeywords = await _context.ServiceCampaigns.GroupBy(k => new 
                                                                            { 
                                                                                k.CampaignId, 
                                                                                k.CampaignKeyword 
                                                                            })
                                                                            .Select(kc => new ServiceCampaign() 
                                                                            { 
                                                                                CampaignId = kc.Key.CampaignId, 
                                                                                CampaignKeyword = kc.Key.CampaignKeyword
                                                                            }).ToListAsync();
                    DictCampaignKeywords = CampaignKeywords.ToDictionary(x => x.CampaignId, x => x.CampaignKeyword);
                }
                else
                {
                    DictCampaignKeywords.Add(0, "No Campaign Keyword Found");
                }
                
                return DictCampaignKeywords;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
