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
    public class GetRenewalMessageHandler : IRequestHandler<GetRenewalMessage, Message>
    {
        private readonly IRediSmsDbContext _context;

        public GetRenewalMessageHandler(IRediSmsDbContext context)
        {
            _context = context;
        }

        public async Task<Message> Handle(GetRenewalMessage request, CancellationToken cancellationToken)
        {
            try
            {
                //If Push Message is empty
                if (String.IsNullOrEmpty(request.rMessage.MessageTxt))
                {
                    var iContent = await _context.Contents.Where(c => c.MessageId.Equals(request.rMessage.MessageId) && c.ContentSchedule.Equals(DateTime.Today)).FirstOrDefaultAsync();
                    if(request.rMessage.IsRichContent)
                    {
                        //HOMEWORK: replace with Get content API URL later
                        request.rMessage.MessageTxt = iContent.ContentText;
                    }
                    else
                    {
                        request.rMessage.MessageTxt = iContent.ContentText;
                    }

                    iContent.Processed = true;
                    _context.Contents.Update(iContent);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                
                return request.rMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
