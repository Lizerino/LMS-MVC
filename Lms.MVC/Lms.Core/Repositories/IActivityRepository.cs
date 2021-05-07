using System.Collections.Generic;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.Core.Repositories
{
    public interface IActivityRepository
    {
        Task AddAsync<T>(T added);
        Task<IEnumerable<Course>> GetAllActivitiesAsync();
        Task<Course> GetActivityAsync(int? id);
        void Remove(Course removed);
        void Remove<T>(T removed);
        Task<bool> SaveAsync();
        Task<bool> ActivityExists(int id);
        void Update(Course course);

    }
}