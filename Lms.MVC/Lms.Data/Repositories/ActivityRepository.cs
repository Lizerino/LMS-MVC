using Lms.MVC.Data.Data;
using Lms.MVC.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.MVC.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lms.MVC.Data.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext db;

        public ActivityRepository(ApplicationDbContext db) => this.db = db;

        public async Task<IEnumerable<Activity>> GetAllActivities() => await db.Activities.ToListAsync();

        //public async Task<Activity> GetActivity(int? Id) => await db.Activities.AddAsync();//(int ? id);
        public async Task AddAsync<T>(T added) => await db.AddAsync(added);

        public void Remove<T>(T removed) => db.Remove(removed);


        public Task<T> GetT<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetTs<T>()
        {
            throw new NotImplementedException();
        }
    }
}
