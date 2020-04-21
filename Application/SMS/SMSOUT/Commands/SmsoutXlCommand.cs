using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.SMSOUT.Commands
{
    public class SmsoutXlCommand : IRequest<SmsoutTelcoHitVm>
    {
        public SmsoutD Smsout { get; set; }
    }
}
