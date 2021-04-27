using Lms.MVC.Core.Entities;
using Lms.MVC.Data.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Lms.MVC.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var config = services.GetRequiredService<IConfiguration>();

                    // TODO: REMOVE IN PRODUCTION
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    var adminPW = config["AdminPW"];

                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    CreateRoles.Create(roleManager);
                    SeedData seedData = new SeedData(db);

                    seedData.Seed(userManager, roleManager);

                    try
                    {
                        CreateAdmin.CreateAdminAsync(services, adminPW).Wait();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex.Message, "Seed Fail");
                    }

                    host.Run();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}