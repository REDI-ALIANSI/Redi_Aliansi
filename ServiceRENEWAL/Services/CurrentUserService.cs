using Application.Common.Interfaces;

namespace ServiceRENEWAL.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return "RenewalWorker";
        }
    }
}
