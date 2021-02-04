using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerTestBackupLiveTable.Services
{
    class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return "Worker Test Backup Live Table";
        }
    }
}
