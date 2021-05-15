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
        Task<IEnumerable<string>> GetAllLateAssignmentsFromModuleAsync(int id);
        IEnumerable<string> GetAllLateAssignmentsFromCourseAsync(int courseId);
        Task<bool> SaveAsync();
        Task<IEnumerable<Activity>> GetAllActivitiesFromModuleAsync(int id);
        string GetNextDueAssignment();
        Task<bool> ActivityExists(int id);

        void Update(Activity activity);

        Task<IEnumerable<ActivityType>> GetAllActivityTypesAsync();
        Task<ICollection<ApplicationFile>> GetAllFilesByActivityId(int id);
        Task<List<Activity>> GetAllActivitiesByModuleIdAsync(int id);
    }
}