﻿using System.Collections.Generic;
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

        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }

        public void Remove<T>(T removed) => db.Remove(removed);

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

        public async Task<bool> CourseExists(int id)
        {
            return await db.Courses.AnyAsync(c=>c.Id==id);
        }

        public void Update(Course course)
        {
            db.Update(course);
        }
       public async Task<DateTime> CalculateEndDate(int id)
        {
            
            var course = await GetCourseAsync(id);
            var modulesEndDates = course.Modules.Select(m => m.EndDate).ToList();
            var endDate = modulesEndDates.Last();
            foreach (var date in modulesEndDates)
            {
                if (date > endDate )
                {
                    endDate = date;
                }

            }

            return endDate;
        }

     
        public async void SetAllCoursesEndDate()
        {
            var courses = await GetAllCoursesAsync(true);
            foreach (var course in courses)
            {
                if (course.Modules.Any())
                {
                course.EndDate = await CalculateEndDate(course.Id);
                }
                else
                {
                    course.EndDate = course.StartDate;
                }
            }
        }
    }
}
