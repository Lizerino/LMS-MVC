using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;

using Microsoft.EntityFrameworkCore;

namespace Lms.MVC.Data.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext db;

        public ActivityRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Activity> GetActivityWithFilesAsync(int? id) => await db.Activities.Include(c => c.Files).Where(c => c.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Activity added) => await db.AddAsync(added);

        public void Remove(Activity removed) => db.Remove(removed);

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync() => await db.Activities.ToListAsync();

        public async Task<IEnumerable<Activity>> GetAllActivitiesFromModuleAsync(int id) => await db.Activities.Where(a => a.ModuleId == id).ToListAsync();
        public async Task<IEnumerable<string>> GetAllLateAssignmentsFromModuleAsync(int id) => await db.Activities.Where(a => a.ModuleId == id).Where(a=>a.EndDate < DateTime.Now)
            
            .Where(m=>m.ActivityType.Name == "Assignment").Select(m=>m.Title)
            .ToListAsync();
        public  IEnumerable<string> GetAllLateAssignmentsFromCourseAsync(int courseId)
        {
            var titles = db.Courses.Include(c => c.Modules).ThenInclude(m => m.Activities).ThenInclude(a => a.ActivityType)
                .FirstOrDefaultAsync(c => c.Id == courseId).Result.Modules.Select(m => m.Activities).SelectMany(a => a.Select(a => a.ActivityType)
                  .Select(at => at.Name));


            return titles;




            //var result = db.Courses.FirstOrDefault(c => c.Id == courseId).Modules
            //    .Select(m => m.Activities
            //    .Where(a => a.EndDate < DateTime.Now && a.ActivityType.Name == "Assignment")
            //    .Select(a => a.Title));



            //var course = db.Courses.FirstOrDefault(c => c.Id == courseId);
            //var modules = course.Modules;
            //var activities = modules.Select(m=>m.Activities)
            //    .Where(a=>a.Where(a=>a.ActivityType.Name == "Assignment" && a.EndDate < DateTime.Now).)
                



            //var result2 = db.Courses.FirstOrDefault(c => c.Id == courseId).Modules

            //    var what = result2
            //    .Where(m => m.Activities.Select(a=>a.ActivityType.Name== "Assignment").FirstOrDefault())
                //.Where(a => a.ActivityType.Name == "Assignment" && a.EndDate < DateTime.Now))


             //.Select(m => m.Activities
             //.Where(a => a.EndDate < DateTime.Now && a.ActivityType.Name == "Assignment"))
             //.Select(a => a.Title);




            //var test = db.Activities.Where(a => a.ActivityType.Name == "Assignment" && a.EndDate < DateTime.Now).Where(a => a.)
            //var result2 = db.Modules.Where(m => m.CourseId == courseId).Where(m => m.Activities.Where(a => a.ActivityType.Name == "Assignment")).Select(a => a.);





            //Include(c=>c.Modules).ThenInclude(m=>m.Activities)
            //.ThenInclude(a=>a.ActivityType.
        }



        // db.Modules.Include(m=>m.Activities).ThenInclude(a=>a.ActivityType)
        //.Where(m => m.CourseId == courseId).FirstOrDefaultAsync(m => m.Id == moduleId).Result.Activities.Where(a => a.EndDate < DateTime.Now)
        //.Where(a => a.ActivityType.Name == "Assignment")
        //    .Select(a => a.Title);



        public async Task<Activity> GetActivityAsync(int? id) => await db.Activities.FirstOrDefaultAsync(c => c.Id == id);
       

        public async Task<bool> SaveAsync() => (await db.SaveChangesAsync()) >= 0;

        public async Task<bool> ActivityExists(int id) => await db.Activities.AnyAsync(c => c.Id == id);

        public void Update(Activity activity) => db.Update(activity);

        public async Task<IEnumerable<ActivityType>> GetAllActivityTypesAsync() => await db.ActivityTypes.ToListAsync();

        public async Task<ICollection<ApplicationFile>> GetAllFilesByActivityId(int id)
        {
            var activity = await db.Activities.Where(c => c.Id == id).Include(c => c.Files).FirstOrDefaultAsync();
            return activity.Files;
        }

        public string GetNextDueAssignment() => db.Activities.Where(a => a.ActivityType.Name.ToLower() == "assignment")
                .FirstOrDefaultAsync(a => a.StartDate <= DateTime.Now && a.EndDate > DateTime.Now).Result.Title;

    }
}