using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Presistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<RediSmsDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("RediAliansi")));

            services.AddScoped<IRediSmsDbContext>(provider => provider.GetService<RediSmsDbContext>());

            return services;
        }
    }
}
