using MediatR;

namespace Application.Common.Behaviour
{
    public class CheckReservedKeyword : IRequest<bool>
    {
        public string Mo_Message { get; set; }
    }
}
