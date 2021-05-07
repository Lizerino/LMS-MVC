using System.Collections.Generic;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.Core.Repositories
{
    public interface IActivityRepository
    {
        Task AddAsync<T>(T added);
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        Task<Activity> GetActivityAsync(int? id);
        void Remove(Activity removed);
        void Remove<T>(T removed);
        Task<bool> SaveAsync();
        Task<bool> ActivityExists(int id);
        void Update(Activity activity);

    }
}