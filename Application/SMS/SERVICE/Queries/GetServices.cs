using System.Collections.Generic;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.SERVICE.Queries
{
    public class GetServices : IRequest<List<Service>>
    {
    }
}
