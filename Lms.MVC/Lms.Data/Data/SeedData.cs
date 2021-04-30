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

        public int numberOfCourses { get; set; }

        public int numberOfModules { get; set; }

        public int numberOfModulesPerCourse { get; set; }

        public int numberOfActivities { get; set; }

        public int numberOfActivititesPerModule { get; set; }

        public int numberOfStudents { get; set; }

        public int numberOfStudentsPerClass { get; set; }

        public int numberOfTeachers { get; set; }
        public int numberOfTeachersPerClass { get; set; }

        public SeedData(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;

            // Set Random to a fixed number to generate the same data each time Randomizer.Seed =
            // new Random(12345);
            Randomizer.Seed = new Random();

            numberOfCourses = 5;
            numberOfModulesPerCourse = 3;
            numberOfActivititesPerModule = 3;
            numberOfStudentsPerClass = 10;
            numberOfTeachersPerClass = 1;

            numberOfModules = numberOfCourses * numberOfModulesPerCourse;
            numberOfActivities = numberOfModules * numberOfActivititesPerModule;
            numberOfStudents = numberOfCourses * numberOfStudentsPerClass;
            numberOfTeachers = numberOfCourses;
        }

        public void Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            for (int i = 0; i < numberOfCourses; i++)
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

            for (int i = 0; i < numberOfModulesPerCourse; i++)
            {
                course.Modules.Add(GetModule());
            }

            course.Users = new List<ApplicationUser>();

            for (int i = 0; i < numberOfStudentsPerClass; i++)
            {
                course.Users.Add(GetStudent());
            }

            for (int i = 0; i < numberOfTeachersPerClass; i++)
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
            for (int i = 0; i < numberOfActivititesPerModule; i++)
            {
                module.Activities.Add(GetActivity());
            }

            return module;
        }

        private Activity GetActivity()
        {
            var fake = new Faker("sv");

            var ran = fake.Random.Int(0, 4);
            var activity = new Activity
            {
                Title = fake.Name.JobTitle(),
                StartDate = fake.Date.Soon(),
                Description = fake.Lorem.Sentence(),
                ActivityType = GetActivityType(ran)
            };

            return activity;
        }

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

        private static ActivityType GetActivityType(int ran)
        {
            var activityTypeEnum = (ActivityTypeEnum)ran;
            var activityTypeName = activityTypeEnum.ToString();
            var activityType = new ActivityType
            {
                Name = activityTypeName
            };
            return activityType;
        }
    }

    public enum ActivityTypeEnum
    {
        Lecture,

        ELearning,

        Practise,

        Assignment,

        Other
    }
}