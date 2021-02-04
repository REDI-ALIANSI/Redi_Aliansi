using Application.Common.Interfaces;

namespace ServiceSMSOUT.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return "SMSOUT WORKER";
        }
    }
}
