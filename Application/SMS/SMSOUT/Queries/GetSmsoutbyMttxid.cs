using MediatR;
using Domain.Entities.SMS;

namespace Application.SMS.SMSOUT.Queries
{
    public class GetSmsoutbyMttxid : IRequest<SmsoutD>
    {
        public string MttxId { get; set; }
    }
}
