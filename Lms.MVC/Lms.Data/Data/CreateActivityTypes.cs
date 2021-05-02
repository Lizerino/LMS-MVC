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

        public CreateActivityTypes(ApplicationDbContext db)
        {
            this.db = db;
        }
        public static void Create(List<ActivityType> activityType)
        {
            Array values = new string[] { "Lecture", "ELearning", "Practise", "Assignment", "Other" };
            var type = new ActivityType();

            for (int i = 0; i < values.Length; i++)
            {
                type.Id = i;
                //type.Name = values[i];
                //db.activityType.add(type);
            }

        }


    }
}