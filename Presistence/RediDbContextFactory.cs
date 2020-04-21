using Microsoft.EntityFrameworkCore;

namespace Presistence
{
    public class RediDbContextFactory : DesignTimeDbContextFactoryBase<RediSmsDbContext>
    {
        protected override RediSmsDbContext CreateNewInstance(DbContextOptions<RediSmsDbContext> options)
        {
            return new RediSmsDbContext(options);
        }
    }
}
