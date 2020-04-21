using MediatR;
using System;

namespace Application.Common.Behaviour
{
    public class GetNextDayofWeekDate : IRequest<DateTime>
    {
        public DayOfWeek? DayofWeek { get; set; }
    }
}
