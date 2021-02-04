using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebSmsin.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetUserId()
        {
            return "SMSIN API";
        }
    }
}
