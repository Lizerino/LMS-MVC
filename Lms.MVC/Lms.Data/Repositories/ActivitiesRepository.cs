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

        public void Remove(Course removed)
        {
            db.Remove(removed);
        }

        public async Task<IEnumerable<Course>> GetAllActivitiesAsync()
        {
            return await db.Courses.ToListAsync();
        }

        public async Task<Course> GetActivityAsync(int? id)
        {
            var query = db.Courses.AsQueryable();
            return await query.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == id);
        }

        //public async Task<Course> GetCourseByTitleAsync(string title)
        //{
        //    var query =  db.Courses.AsQueryable();
        //    return await query.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Title == title);
        //}

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }

        public async Task<bool> ActivityExists(int id)
        {
            return await db.Activities.AnyAsync(c => c.Id == id);
        }

        public void Update(Course course)
        {
            db.Update(course);
        }
    }
}