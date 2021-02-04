using Application.Common.Interfaces;

namespace WorkerGenerateDailyReports.Services
{
    class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return "Worker Generate Daily Report";
        }
    }
}
