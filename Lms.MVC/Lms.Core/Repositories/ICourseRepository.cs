using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.Core.Repositories
{
    public interface ICourseRepository
    {
        Task AddAsync<T>(T added);
        Task<IEnumerable<Course>> GetAllCoursesAsync(bool includeModules=false, bool includeUsers = false);
        Task<Course> GetCourseAsync(int? id, bool includeModules=false, bool includeUsers = false);
        void Remove(Course removed);
        void Remove<T>(T removed);
        Task<bool> SaveAsync();
        Task<bool> CourseExists(int id);
        void Update(Course course);
        Task<DateTime> CalculateEndDateAsync(int id);
        void SetAllCoursesEndDate();
        Task<Course> SetCourseEndDateAsync(int id);
    }
}