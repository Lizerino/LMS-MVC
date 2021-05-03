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
        public static void Create(ApplicationDbContext db)
        {
            if (db.ActivityTypes.Select(a=>a.Id).Count() == 0)       
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