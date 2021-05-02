using Lms.MVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Data.Data
{
    public class CreateActivityTypes
    {
        private readonly ApplicationDbContext db;

        public ApplicationDbContext Db => db;

        //public CreateActivityTypes(ApplicationDbContext db)
        //{
        //    this.db = db;
        //}
        //public static void Create(List<ActivityType> activityType)
        //{
        //    Array values = new string[] { "Lecture", "ELearning", "Practise", "Assignment", "Other" };
        //    var type = new ActivityType();
        //
        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        type.Id = i;
        //        type.Name = values[i];
        //        db.activityType.add(type);
        //    }
        //    db.SaveChanges();
        //
        //}
        public static void Create(ApplicationDbContext db)
        {
            //if (db.ActivityTypes.Select(a => a.Id).Count() != 0) db.ActivityTypes.Remove;
            if (db.ActivityTypes.Select(a => a.Id).Count() == 0)
            {
                var ActivityTypeList = new List<string>()
            {
                "Lecture",
                "ELearning",
                "Practise",
                "Assignment",
                "Other"
            };

                foreach (var activityType in ActivityTypeList)
                {
                    var type = new ActivityType();
                    type.Name = activityType;
                    db.ActivityTypes.Add(type);
                }
                db.SaveChanges();
            }
        }

    }
}