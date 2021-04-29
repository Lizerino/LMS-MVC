using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Data.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return
                        await db.Users.ToListAsync();
        }

        public string GetRole(ApplicationUser user)
        {
            var sb = new StringBuilder();
            var roles = userManager.GetRolesAsync(user).Result.ToList();
            
            foreach (var item in roles)
            {
                sb.AppendLine(item);
            }
            var rolesName = sb.ToString();
            return rolesName;
        }
    }
}
