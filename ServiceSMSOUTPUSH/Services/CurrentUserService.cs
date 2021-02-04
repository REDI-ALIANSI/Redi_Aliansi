using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;

namespace ServiceSMSOUTPUSH.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return "SMSOUTPUSH WORKER";
        }
    }
}
