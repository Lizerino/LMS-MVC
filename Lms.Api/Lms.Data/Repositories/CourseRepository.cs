using Lms.MVC.Core.Entities;
using Lms.API.Core.Repositories;
using Lms.MVC.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Data.Repositories
{
    class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext db;
       


        public CourseRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        } 
        public void Remove(Course removed)
        {
             db.Remove(removed);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync(bool includeModules)
        {

            return includeModules ? await db.Courses
                        .Include(l => l.Modules)
                        .ToListAsync() :
                        await db.Courses
                        .ToListAsync();


        }

        public async Task<Course> GetCourseAsync(int? id)
        {
            var query =  db.Courses.AsQueryable();
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

       
    }
}
