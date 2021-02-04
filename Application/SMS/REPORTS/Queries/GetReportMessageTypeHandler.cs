using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.REPORTS.Queries
{
    public class GetReportMessageTypeHandler : IRequestHandler<GetReportMessageType, Dictionary<int, string>>
    {
        public Task<Dictionary<int, string>> Handle(GetReportMessageType request, CancellationToken cancellationToken)
        {
            Dictionary<int, string> MessageTypes = new Dictionary<int, string>()
                { { 0,"ALL"}, { 1,"SUBS" }, { 2,"PUSH" }, { 3,"PULL" } };

            return Task.FromResult(MessageTypes);
        }
    }
}
