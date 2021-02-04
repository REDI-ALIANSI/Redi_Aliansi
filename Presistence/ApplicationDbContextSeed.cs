using Presistence.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Presistence
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RediSmsDbContext context)
        {
            string[] roles = new string[] { "Administrator", "InternalManager", "TelcoAdm", "CampaignManager", "CustomerService", "ContentAdm", "ReportAdm"};
            var defaultUser = new ApplicationUser { UserName = "febrian.alfachri@redialiansi.com", Email = "febrian.alfachri@redialiansi.com" };

            foreach (string role in roles)
            { 
                var roleStore = new RoleStore<IdentityRole>(context);
                
                if (!context.Roles.Any(r => r.Name == role))
                {
                    await roleStore.CreateAsync(new IdentityRole(role));
                }
            }

            var irole = context.Roles.SingleOrDefault(r => r.Name == "Administrator");
            var user = context.Users.SingleOrDefault(u => u.UserName == defaultUser.UserName);

            if (user == null)
            {
                IdentityResult result = await userManager.CreateAsync(defaultUser, "Exc4l1bur!");
                user = context.Users.SingleOrDefault(u => u.UserName == defaultUser.UserName);
                if (irole != null)
                {
                    await userManager.AddToRoleAsync(user, irole.Name);
                }
            }
            else
            {
                if (user.UserName == defaultUser.UserName)
                {
                    var UserRole = userManager.GetRolesAsync(user).Result.FirstOrDefault();
                    if (UserRole != irole.Name)
                    {
                        await userManager.AddToRoleAsync(user, irole.Name);
                    }
                }
            }
        }
    }
}
