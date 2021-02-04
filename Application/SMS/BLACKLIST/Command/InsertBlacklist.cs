using Application.Common.Interfaces;
using Application.Common.Model;
using Application.SMS.BLACKLIST.Query;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.BLACKLIST.Command
{
    public class InsertBlacklist : IRequest<Result>
    {
        public string Msisdn { get; set; }
        public int OperatorId { get; set; }
    }

    public class InsertBlacklistHandler : IRequestHandler<InsertBlacklist, Result>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;
        public InsertBlacklistHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<Result> Handle(InsertBlacklist request, CancellationToken cancellationToken)
        {
            try
            {
                if(await _mediator.Send(new IsBlacklist { Msisdn = request.Msisdn, OperatorId = request.OperatorId }))
                {
                    return Result.Failure(new string[] { "Msisdn Already Blacklisted" });
                }
                else
                {
                    var newBlackList = new BlackList()
                    {
                        Msisdn = request.Msisdn,
                        OperatorId = request.OperatorId
                    };
                    _context.BlackLists.Add(newBlackList);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success();
                }
            }
            catch(Exception ex)
            {
                return Result.Failure(new string[] { ex.Message });
            }
        }
    }
}
