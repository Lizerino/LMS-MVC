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
        Task<IEnumerable<Activity>> GetAllActivities();
        //Task<Activity> GetActivity(Id);
        Task<T> GetT<T>();
        Task<IEnumerable<T>> GetTs<T>();
        Task AddAsync<T>(T added);
        void Remove<T>(T Removed);

    }
}
