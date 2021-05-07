using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;

using Microsoft.EntityFrameworkCore;

namespace Lms.MVC.Data.Repositories
{
    internal class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext db;

        public ModuleRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }

        public async Task<IEnumerable<Module>> GetAllModulesAsync()
        {
            var query = db.Modules.AsQueryable();

            return await query.ToArrayAsync();
        }

        public async Task<IEnumerable<Module>> GetAllModulesAsync(int id)
        {
            var query = db.Modules.AsQueryable().Where(m => m.CourseId == id);             

            return await query.ToArrayAsync();
        }

        public async Task<Module> GetModuleAsync(int id, int moduleId)
        {
            var query = db.Modules.AsQueryable();
            return await query.FirstOrDefaultAsync(m => m.Id == moduleId && m.CourseId == id);
        }

        public async Task<Module> GetModuleByTitleAsync(int id, string title)
        {
            var query = db.Modules.AsQueryable();
            return await query.FirstOrDefaultAsync(m => m.Title == title && m.CourseId == id);
        }

        public void Remove(Module removed)
        {
            db.Remove(removed);
        }
        public void Remove<T>(T removed) => db.Remove(removed);
        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
    }
}