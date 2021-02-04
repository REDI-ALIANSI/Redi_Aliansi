using System.Collections.Generic;
using Domain.Entities;
using MediatR;

namespace Application.SMS.OPERATOR.Queries
{
    public class GetOperators : IRequest<List<Operator>>
    { 
    }
}
