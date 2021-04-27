using System;

using Lms.MVC.Core.Entities;
using Lms.MVC.Data.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                    var confiq = services.GetRequiredService<IConfiguration>();
                    context.Database.EnsureDeleted();
                    context.Database.Migrate();
                    //SeedData seed = new SeedData(db);
                    //seed.Seed();

                    var adminPW = confiq["AdminPW"];
                    try
                    {
                        CreateAdmin.CreateAdminAsync(services, adminPW).Wait();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex.Message, "Seed Fail");
                using (var scope = host.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var serviceProvider = scope.ServiceProvider;
                    
                    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    // TODO: REMOVE IN PRODUCTION
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    CreateRoles.Create(roleManager);
                    SeedData seedData = new SeedData(db);

                    seedData.Seed(userManager, roleManager);
                }
            }
            catch (Exception)
            {
                throw;
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}