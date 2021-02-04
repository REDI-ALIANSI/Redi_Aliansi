using Application.Common.Interfaces;
using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
           //services.AddScoped<IUserManager, UserManagerService>();
            services.AddTransient<IDateTime, MachineDateTime>();
            services.AddTransient<IMsgQ, MsgQ>();
            services.AddTransient<IExecuteDllService, ExecuteDllService>();
            services.AddTransient<IHttpRequest, HttpRequest>();
            services.AddTransient<IShortenURL, Bitly_url>();

            return services;
        }
    }
}
