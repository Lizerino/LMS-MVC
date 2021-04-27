using Lms.MVC.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Data.Data
{
    public class CreateAdmin
    {

        public static async Task CreateAdminAsync(IServiceProvider services, string adminPW)
        {
            using (var context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                var roleName = "Admin";
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole { Name = roleName };
                    var result = await roleManager.CreateAsync(role);

                    if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
                }
                var adminEmail = "admin@lms.se";
                var foundAdmin = await userManager.FindByEmailAsync(adminEmail);

                if (foundAdmin != null) return;

                var admin = new User
                {
                    UserName = "admin@admin.se",
                    Email = adminEmail,
                   
                };
                var addAdminResult = await userManager.CreateAsync(admin, adminPW);

                if (!addAdminResult.Succeeded) throw new Exception(string.Join("\n", addAdminResult.Errors));

                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (!await userManager.IsInRoleAsync(adminUser, roleName))
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, roleName);
                    if (!addToRoleResult.Succeeded) throw new Exception(string.Join("\n", addToRoleResult.Errors));

                }

                await context.SaveChangesAsync();
            }


        }
    }
}
