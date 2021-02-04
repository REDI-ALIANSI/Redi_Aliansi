using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.CONTENT.Queries
{
    public class GetContentbyId : IRequest<Content>
    {
        public int ContentId { get; set; }
    }
}
