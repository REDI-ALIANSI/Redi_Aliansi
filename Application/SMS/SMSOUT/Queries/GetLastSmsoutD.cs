using MediatR;
using Domain.Entities.SMS;
using System.Threading.Tasks;
using System.Threading;
using Application.Common.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.SMS.SMSOUT.Queries
{
    public class GetLastSmsoutD : IRequest<SmsoutD>
    {
        public string Msisdn { get; set; }
        public int OperatorId { get; set; }
    }

    public class GetLastSmsoutDHandler : IRequestHandler<GetLastSmsoutD, SmsoutD>
    {
        private readonly IRediSmsDbContext _context;

        public GetLastSmsoutDHandler(IRediSmsDbContext context)
        {
            _context = context;
        }
        public async Task<SmsoutD> Handle(GetLastSmsoutD request, CancellationToken cancellationToken)
        {
            return await _context.SmsoutDs.Where(s => s.Msisdn.Equals(request.Msisdn)
                                                    && s.OperatorId.Equals(request.OperatorId))
                                                    .OrderByDescending(s => s.DateProcessed)
                                                    .FirstOrDefaultAsync();
        }
    }
}
