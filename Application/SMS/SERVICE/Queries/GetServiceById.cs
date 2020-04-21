using Domain.Entities.SMS;
using MediatR;
using System.Collections.Generic;

namespace Application.SMS.SERVICE.Queries
{
    public class GetServiceById : IRequest<Service>
    {
        public int ServiceId { get; set; }
    }
}
