using MediatR;
using System.Collections.Generic;

namespace Application.SMS.REPORTS.Queries
{
    public class GetReportMessageType : IRequest<Dictionary<int,string>>
    {

    }
}
