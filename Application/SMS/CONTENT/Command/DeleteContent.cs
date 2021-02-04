using Application.Common.Model;
using MediatR;

namespace Application.SMS.CONTENT.Command
{
    public class DeleteContent : IRequest<Result>
    {
        public int ContentId { get; set; }
    }
}
