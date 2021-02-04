using Application.Common.Model;
using System;
using MediatR;

namespace Application.SMS.REPORTS.Commands
{
    public class GenerateRevenueReports : IRequest<Result>
    {
        public DateTime GenDate { get; set; }
    }
}