using System;
using System.Collections.Generic;

using Bogus;

using Lms.MVC.Core.Entities;

using Microsoft.AspNetCore.Identity;

namespace Lms.MVC.Data.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> userManager;

        public int NumberOfCourses { get; set; }
                   
        public int NumberOfModules { get; set; }
                   
        public int NumberOfModulesPerCourse { get; set; }
                   
        public int NumberOfActivities { get; set; }
                   
        public int NumberOfActivititesPerModule { get; set; }
                   
        public int NumberOfStudents { get; set; }
                   
        public int NumberOfStudentsPerClass { get; set; }
                   
        public int NumberOfTeachers { get; set; }
        public int NumberOfTeachersPerClass { get; set; }

        public SeedData(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;

            // Set Random to a fixed number to generate the same data each time Randomizer.Seed =
            // new Random(12345);
            Randomizer.Seed = new Random();

            NumberOfCourses = 5;
            NumberOfModulesPerCourse = 3;
            NumberOfActivititesPerModule = 3;
            NumberOfStudentsPerClass = 10;
            NumberOfTeachersPerClass = 1;
            
            NumberOfModules = NumberOfCourses * NumberOfModulesPerCourse;
            NumberOfActivities = NumberOfModules * NumberOfActivititesPerModule;
            NumberOfStudents = NumberOfCourses * NumberOfStudentsPerClass;
            NumberOfTeachers = NumberOfCourses;
        }

        public void Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            for (int i = 0; i < NumberOfCourses; i++)
            {
                db.Courses.Add(GetCourse());
            }
            db.SaveChanges();
        }

        private Course GetCourse()
        {
            var fake = new Faker("sv");
            var course = new Course();

            course.Title = fake.Company.CatchPhrase();
            course.StartDate = DateTime.Now.AddDays(fake.Random.Int(-2, 2));

            course.Modules = new List<Module>();

            for (int i = 0; i < NumberOfModulesPerCourse; i++)
            {
                course.Modules.Add(GetModule());
            }

            course.Users = new List<ApplicationUser>();

            for (int i = 0; i < NumberOfStudentsPerClass; i++)
            {
                course.Users.Add(GetStudent());
            }

            for (int i = 0; i < NumberOfTeachersPerClass; i++)
            {
            course.Users.Add(GetTeacher());            
            }


            return course;
        }

        private Module GetModule()
        {
            var fake = new Faker("sv");

            var module = new Module();

            module.Title = fake.Name.JobTitle();
            module.StartDate = fake.Date.Soon();

            module.Activities = new List<Activity>();
            for (int i = 0; i < NumberOfActivititesPerModule; i++)
            {
                module.Activities.Add(GetActivity());
            }

            return module;
        }

        private Activity GetActivity()
        {
            var fake = new Faker("sv");

            fake = new Faker("sv");
            var ran = fake.Random.Int(0, 4);
            var startdtime = DateTime.Now.AddDays(fake.Random.Int(0, 7));
            var activity = new Activity
            {
                Title = fake.Name.JobTitle(),
                StartDate = startdtime,
                EndDate = startdtime.AddHours(fake.Random.Int(1, 8)),
                Description = fake.Lorem.Sentence(),
                //ActivityType = //GetActivityType(ran),
                //ModuleId = //GetModuleIdForActivity()
                                                   //ActivityTypeId = i,
                                                   //Id = i,
            };

            return activity;
        }
        //private static Int32 i = 0;
        //private static Int32 GetModuleIdForActivity()
        //{
        //        i++;
        //        return i; 
        //}

        //private static List<Activity> GetActivities()
        //{
        //    Faker fake;
        //    var activitys = new List<Activity>();
        //    for (int i = 0; i < 45; i++)
        //    {
        //        fake = new Faker("sv");
        //        var ran = fake.Random.Int(0, 4);
        //        var startdtime = DateTime.Now.AddDays(fake.Random.Int(0, 7));
        //
        //        var activity = new Activity
        //        {
        //            Title = fake.Name.JobTitle(),
        //            StartDate = startdtime,
        //            EndDate = startdtime.AddHours(fake.Random.Int(1, 8)),
        //            Description = fake.Lorem.Sentence(),
        //            ActivityType = GetActivityType(ran)//,
        //                                               //ActivityTypeId = i,
        //                                               //Id = i,
        //                                               //ModuleId = i
        //        };
        //        activitys.Add(activity);
        //    }
        //    return activitys;
        //}


        private ApplicationUser GetTeacher()
        {
            var fake = new Faker("sv");

            var teacher = new ApplicationUser();

            teacher.Name = fake.Name.FullName();
            teacher.Email = fake.Internet.Email();
            teacher.UserName = teacher.Email;
            teacher.Role = "Teacher";

            userManager.CreateAsync(teacher, "password").Wait();
            userManager.AddToRoleAsync(teacher, "Teacher").Wait();

            return teacher;
        }

        private ApplicationUser GetStudent()
        {
            var fake = new Faker("sv");

            var student = new ApplicationUser();

            student.Name = fake.Name.FullName();
            student.Email = fake.Internet.Email();
            student.UserName = student.Email;
            student.Role = "Student";

            userManager.CreateAsync(student, "password").Wait();
            userManager.AddToRoleAsync(student, "Student").Wait();

            return student;
        }

        //private static ActivityType GetActivityType()
        //{
        //    Array values = Enum.GetValues(typeof(ActivityTypeEnum));
        //    Random random = new();
        //    ActivityType r = (ActivityType)values.GetValue(random.Next(values.Length));
        //    return r;
        //}
   //     private static ActivityType GetActivityType(int ran)
   //     {
   //         var activityTypeEnum = (ActivityTypeEnum)ran;
   //         var activityTypeName = activityTypeEnum.ToString();
   //         var activityType = new ActivityType
   //         {
   //             Name = activityTypeName
   //         };
   //         return activityType;
   //     }
   // }
   //
   // public enum ActivityTypeEnum
   // {
   //     Lecture,
   //
   //     ELearning,
   //
   //     Practise,
   //
   //     Assignment,
   //
   //     Other
   // }
}