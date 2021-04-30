using Lms.MVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Core.Repositories
{
    public interface IUserRepository
    {

        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        string GetRole(ApplicationUser user);
        Task<ApplicationUser> FindAsync(string id);
        void Update(ApplicationUser user);
        public bool Any(string id);
    }
}
