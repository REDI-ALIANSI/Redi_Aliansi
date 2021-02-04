using Application.Common.Interfaces;

namespace WorkerBackupLiveTable.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return "Worker Backup Live Table";
        }
    }
}
