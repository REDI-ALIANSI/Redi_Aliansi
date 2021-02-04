using Application.Common.Interfaces;

namespace WebSmsDn.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return "SMSDN API";
        }
    }
}
