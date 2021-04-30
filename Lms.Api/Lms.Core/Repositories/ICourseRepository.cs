using Lms.MVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Core.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync(bool includeModules);
        Task<Course> GetCourseAsync(int? id);
     //    Task<Course> GetCourseByTitleAsync(string title);
        void Remove(Course removed);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
    }
}
