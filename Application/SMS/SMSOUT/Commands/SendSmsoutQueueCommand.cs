using Common;
using Domain.Entities.SMS;
using MediatR;


namespace Application.SMS.SMSOUT.Commands
{
    public class SendSmsoutQueueCommand : IRequest
    {
        public Message rMessage { get; set; }
        public string rMsisdn { get; set; }
        public string rSparam { get; set; }
        public int rIparam { get; set; }
        public string rQueue { get; set; }
        public int rServiceId { get; set; }
        public string rMtTxId { get; set; }
        public RabbitMQAuth QueueAuth { get; set; }
    }
}
