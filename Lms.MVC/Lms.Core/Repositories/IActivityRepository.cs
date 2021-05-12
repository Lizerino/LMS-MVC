using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.MVC.Core.Entities;

namespace Lms.MVC.Core.Repositories
{
    public interface IActivityRepository
    {
        Task<IEnumerable<Activity>> GetAllActivities(int id);
        Task<Activity> GetActivity(int? Id);
        Task AddAsync(Activity added);
        void Remove(Activity Removed);

    }
}
