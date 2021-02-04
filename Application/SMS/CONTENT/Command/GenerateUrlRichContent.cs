using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.CONTENT.Command
{
    public class GenerateUrlRichContent : IRequest<string>
    {
        public int MessageId { get; set; }
        public string Msisdn { get; set; }
    }

    public class GenerateUrlRichContentHandler : IRequestHandler<GenerateUrlRichContent, string>
    {
        private readonly IRediSmsDbContext _context;
        public GenerateUrlRichContentHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public Task<string> Handle(GenerateUrlRichContent request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
