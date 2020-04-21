using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Application.Common.Behaviour
{
    public class GetNextDayofWeekDateHandler : IRequestHandler<GetNextDayofWeekDate, DateTime>
    {
        public Task<DateTime> Handle(GetNextDayofWeekDate request, CancellationToken cancellationToken)
        {
            try
            {
                DateTime result = DateTime.Today.AddDays(1);
                while (result.DayOfWeek != request.DayofWeek)
                    result = result.AddDays(1);

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
