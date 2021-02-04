using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerTestGenerateDailyReport.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return "Worker Generate Daily Report";
        }
    }
}
