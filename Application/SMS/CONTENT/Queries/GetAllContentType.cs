using System.Collections.Generic;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.CONTENT.Queries
{
    public class GetAllContentType : IRequest<List<ContentType>>
    {
    }
}
