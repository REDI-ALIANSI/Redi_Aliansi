using Domain.Entities.SMS;
using MediatR;
using System.Collections.Generic;

namespace Application.SMS.MESSAGE.Queries
{
    public class GetMessagesByType : IRequest<List<Message>>
    {
        public string MessageType { get; set; }
        public int Serviceid { get; set; }
        public int OperatorId { get; set; }
    }
}
