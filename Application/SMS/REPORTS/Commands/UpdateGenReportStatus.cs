using Application.Common.Model;
using System;
using MediatR;

namespace Application.SMS.REPORTS.Commands
{
    public class UpdateGenReportStatus : IRequest<Result>
    {
        public bool StatusUpdate { get; set; }
    }
}
