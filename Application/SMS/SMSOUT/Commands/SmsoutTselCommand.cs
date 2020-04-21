using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.SMSOUT.Commands
{
    public class SmsoutTselCommand : IRequest<SmsoutTelcoHitVm>
    {
        public SmsoutD Smsout { get; set; }
    }
}
