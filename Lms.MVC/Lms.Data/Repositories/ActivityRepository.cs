using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;

using Microsoft.EntityFrameworkCore;

namespace Lms.MVC.Data.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext db;

        public ActivityRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }

        public void Remove<T>(T removed) => db.Remove(removed);

        public void Remove(Activity removed)
        {
            db.Remove(removed);
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await db.Activities.ToListAsync();
        }

        public async Task<Activity> GetActivityAsync(int? id)
        {
            var query = db.Activities.AsQueryable();
            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }

        public async Task<bool> ActivityExists(int id)
        {
            return await db.Activities.AnyAsync(c => c.Id == id);
        }

        public void Update(Activity activity)
        {
            db.Update(activity);
        }
    }
}