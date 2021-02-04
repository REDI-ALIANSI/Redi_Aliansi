using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Presistence.Identity;
using Microsoft.AspNetCore.Identity;
using Common;
using Infrastructure;
using Domain.Entities.SMS;
using Presistence.Manual_Connection;

namespace Presistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<RediSmsDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("RediAliansi")));

            services.AddScoped<IRediSmsDbContext>(provider => provider.GetService<RediSmsDbContext>());

            //services.AddDefaultIdentity<ApplicationUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<RediSmsDbContext>();

            //services.AddTransient<IIdentityService, IdentityService>();
            services.AddScoped<IDateTime, MachineDateTime>();
            services.AddSingleton<IPostgreConnection, PostgreQueryManual>();

            //services.AddAuthentication();

            return services;
        }
    }
}
