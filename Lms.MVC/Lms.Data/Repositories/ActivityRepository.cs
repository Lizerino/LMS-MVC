using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.Data.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext db;

        public ActivityRepository(ApplicationDbContext db) => this.db = db;


        public async Task<Activity> GetActivity(int? Id) => await db.Activities.FirstOrDefaultAsync(d => d.Id == Id);
        public async Task AddAsync<T>(T added) => await db.AddAsync(added);

        public void Remove<T>(T removed) => db.Remove(removed);

        public Task<IEnumerable<T>> GetTs<T>()
        {
            throw new NotImplementedException();
        }
        //public async Task<IEnumerable<Activity>> GetAllActivities() => await db.Activities.ToListAsync();

        public async Task<IEnumerable<Activity>> GetAllActivities(int id)
            =>await db.Activities.Where(a => a.ModuleId == id).ToListAsync();
        

        private List<Activity> GetActivities(int id)
        {
            var activities = new List<Activity>();

            foreach (var activity in db.Activities)
            {
                if (activity.ModuleId == id)
                {
                    activities.Add(activity);
                }
            }

            return activities;
        }

        public Task<T> GetT<T>()
        {
            throw new NotImplementedException();
        }

    }
}
