using Domain.Entities.SMS;

namespace Application.SMS.SERVICE.ViewModel
{
    public class CustomServiceRenewalRequest
    {
        public Subscription subscription { get; set; }
        public string message { get; set; }
    }
}
