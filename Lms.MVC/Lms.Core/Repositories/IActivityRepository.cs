using System.Collections.Generic;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.Core.Repositories
{
    public interface IActivityRepository
    {
        Task AddAsync(Activity added);

        void Remove(Activity removed);

        Task<IEnumerable<Activity>> GetAllActivitiesAsync();

        Task<Activity> GetActivityWithFilesAsync(int? id);
        Task<Activity> GetActivityAsync(int? id);

        Task<bool> SaveAsync();

        Task<bool> ActivityExists(int id);

        void Update(Activity activity);

        Task<IEnumerable<ActivityType>> GetAllActivityTypesAsync();
        Task<ICollection<ApplicationFile>> GetAllFilesByActivityId(int id);
    }
}