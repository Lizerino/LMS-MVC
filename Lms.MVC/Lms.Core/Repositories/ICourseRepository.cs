using System.Collections.Generic;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.Core.Repositories
{
    public interface ICourseRepository
    {
        Task AddAsync<T>(T added);
        Task<IEnumerable<Course>> GetAllCoursesAsync(bool includeModules);
        Task<Course> GetCourseAsync(int? id);
        void Remove(Course removed);
        void Remove<T>(T removed);
        Task<bool> SaveAsync();
        Task<bool> CourseExists(int id);
        void Update(Course course)

    }
}