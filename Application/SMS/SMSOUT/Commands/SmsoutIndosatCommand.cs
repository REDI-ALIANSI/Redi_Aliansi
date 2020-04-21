using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.SMSOUT.Commands
{
    public class SmsoutIndosatCommand : IRequest<SmsoutTelcoHitVm>
    {
        public SmsoutD Smsout { get; set; }
    }
}
