//TODO GitFix
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;

using Microsoft.EntityFrameworkCore;

namespace Lms.MVC.Data.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext db;

        public ModuleRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Module> GetModuleWithFilesAsync(int? id) => await db.Modules.Include(c => c.Files).Where(c => c.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Module added) => await db.AddAsync(added);

        public async Task<IEnumerable<Module>> GetAllModulesAsync(bool includeActivities)
        {
            return includeActivities ? await db.Modules
                        .Include(l => l.Activities)
                        .ToListAsync() :
                        await db.Modules
                        .ToListAsync();
        }

        public async Task<IEnumerable<Module>> GetAllModulesAsync(int id)
        {
            var query = db.Modules.AsQueryable().Where(m => m.CourseId == id);

            return await query.ToArrayAsync();
        }

        public async Task<Module> GetModuleAsync(int id)
        {
            return await db.Modules.FindAsync(id);
        }

        public async Task<Module> GetModuleAsync(int id, int moduleId)
        {
            var query = db.Modules.AsQueryable();
            return await query.FirstOrDefaultAsync(m => m.Id == moduleId && m.CourseId == id);
        }

        //public async Task<Module> GetModuleAsync(int moduleId)
        //{
        //    var query = db.Modules.AsQueryable().Include(a => a.Activities);
        //    return await query.FirstOrDefaultAsync(m => m.Id == moduleId);
        //}

        public async Task<Module> GetModuleByTitleAsync(int id, string title)
        {
            var query = db.Modules.AsQueryable();
            return await query.FirstOrDefaultAsync(m => m.Title == title && m.CourseId == id);
        }

        public void Remove(Module removed)
        {
            db.Remove(removed);
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }

        public void Update(Module module)
        {
            db.Update(module);
        }

        public async Task<ICollection<ApplicationFile>> GetAllFilesByModuleId(int id)
        {
            var module = await db.Modules.Where(c => c.Id == id).Include(c => c.Files).FirstOrDefaultAsync();
            return module.Files;
        }
    }
}