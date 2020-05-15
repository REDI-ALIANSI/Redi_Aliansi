using Common;
using MediatR;

namespace Application.SMS.SMSIN.Commands
{
    public class SendSmsinQueueCommand : IRequest
    {
        public string Msisdn { get; set; }
        public string Mo_Message { get; set; }
        public string Motxid { get; set; }
        public int OperatorId { get; set; }
        public int Shortcode { get; set; }
        public RabbitMQAuth QueueAuth { get; set; }
    }
}
