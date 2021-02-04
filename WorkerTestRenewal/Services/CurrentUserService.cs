using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerTestRenewal.Services
{
    class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return "Worker Test Renewal";
        }
    }
}
