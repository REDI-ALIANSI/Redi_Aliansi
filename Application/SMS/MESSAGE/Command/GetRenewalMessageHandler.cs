using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.MESSAGE.Command
{
    public class GetRenewalMessageHandler : IRequestHandler<GetRenewalMessage, string>
    {
        private readonly IRediSmsDbContext _context;

        public GetRenewalMessageHandler(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetRenewalMessage request, CancellationToken cancellationToken)
        {
            try
            {
                //If Push Message is empty
                var MessageTxt = String.Empty;
                if (String.IsNullOrEmpty(request.rMessage.MessageTxt))
                {
                    var iContent = await _context.Contents.Where(c => c.MessageId.Equals(request.rMessage.MessageId) && c.ContentSchedule.Equals(request.rRenewalDate)).FirstOrDefaultAsync();

                    if(iContent != null)
                    {
                        if (request.rMessage.IsRichContent)
                        {
                            //HOMEWORK: replace with Get content API URL later
                            MessageTxt = iContent.ContentText;
                        }
                        else
                        {
                            MessageTxt = iContent.ContentText;
                        }

                        iContent.Processed = true;
                        _context.Contents.Update(iContent);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        MessageTxt = String.Empty;
                    }
                }
                
                return MessageTxt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
