using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;

using Microsoft.EntityFrameworkCore;

namespace Lms.MVC.Data.Repositories
{
    internal class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext db;

        public CourseRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(Course added)
        {
            await db.AddAsync(added);
        }

        public void Remove(Course removed)
        {
            db.Remove(removed);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync(bool includeModules = false, bool includeUsers = false)
        {
            return includeModules ?
                includeUsers ?

                await db.Courses
                        .Include(l => l.Modules)
                        .Include(c => c.Users)
                        .ToListAsync()
                        :
                          await db.Courses
                        .Include(l => l.Modules)
                        .ToListAsync()
                        :
                        includeUsers ?
                        await db.Courses
                        .Include(c => c.Users)
                        .ToListAsync()
                        :
                         await db.Courses
                        .ToListAsync();
        }

        public async Task<Course> GetCourseAsync(int? id, bool includeModules = false, bool includeUsers = false)
        {
            return includeModules ?
                includeUsers ?

                await db.Courses.Where(c => c.Id == id)
                        .Include(l => l.Modules)
                        .Include(c => c.Users).FirstOrDefaultAsync()
                        :
                          await db.Courses.Where(c => c.Id == id)
                        .Include(l => l.Modules).FirstOrDefaultAsync()
                        :
                        includeUsers ?
                        await db.Courses.Where(c => c.Id == id)
                        .Include(c => c.Users).FirstOrDefaultAsync()
                        :
                         await db.Courses.Where(c => c.Id == id).FirstOrDefaultAsync();
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

        public async Task<bool> CourseExists(int id)
        {
            return await db.Courses.AnyAsync(c => c.Id == id);
        }

        public void Update(Course course)
        {
            db.Update(course);
        }

        public async Task<DateTime> CalculateEndDateAsync(int id)
        {
            if (GetCourseAsync(id, true, false).Result.Modules.Count > 0)
            {
                var modulesEndDates = (await GetCourseAsync(id, true, false)).Modules.Select(m => m.EndDate).ToList();
                var endDate = modulesEndDates.Last();
                foreach (var date in modulesEndDates)
                {
                    if (date > endDate)
                    {
                        endDate = date;
                    }
                }

                return endDate;
            }
            return DateTime.Now.AddMonths(1);
        }

        public void SetAllCoursesEndDate()
        {
            var courses = GetAllCoursesAsync(true).Result;
            foreach (var course in courses)
            {
                var id = course.Id;
                if (course.Modules.Any())
                {
                    course.EndDate = CalculateEndDateAsync(id).Result;
                }
                else
                {
                    course.EndDate = course.StartDate;
                }
            }
        }

        public async Task<Course> SetCourseEndDateAsync(int id)
        {
            var course = await GetCourseAsync(id, false, false);
            course.EndDate = await CalculateEndDateAsync(id);

            return course;
        }

        public async Task<ICollection<ApplicationFile>> GetAllFilesByCourseId(int id)
        {
            var course = await db.Courses.Where(c => c.Id == id).Include(c => c.Files).FirstOrDefaultAsync();
            return course.Files;            
        }
    }
}