using System;
using System.Collections.Generic;
using System.Linq;

using Bogus;

using Lms.MVC.Core.Entities;

using Microsoft.AspNetCore.Identity;

namespace Lms.MVC.Data.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext db;

        public SeedData(ApplicationDbContext db)
        {
            this.db = db;

            // Set Random to a fixed number to generate the same data each time Randomizer.Seed =
            // new Random(12345);
            Randomizer.Seed = new Random();
        }

        public void Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var courses = GetCourses();
            var students = GetStudents();
            var teachers = GetTeachers();
            var activities = GetActivities();
            var modules = GetModules();

            // Add students to courses
            var i = 0;
            var step = (students.Count() / courses.Count());
            foreach (var course in courses)
            {
                // need to add course to student
                if (students != null)
                {
                    var list = students.Skip(i).Take(step).ToList();
                    foreach (var item in list)
                    {
                        course.Students.Add(item);
                        item.CourseUserTakes = course;
                    }
                    i = i + step;
                }
            }

            // Add teachers to courses
            i = 0;
            foreach (var course in courses)
            {
                if (course.Teachers != null)
                {
                    var list = teachers.Skip(i).Take(1).ToList();
                    foreach (var item in list)
                    {
                        course.Teachers.Add(item);                         
                        item.CoursesUserTeaches.Add(course);
                    }
                    i++;
                }
            }

            // Add modules to courses
            i = 0;
            step = modules.Count() / courses.Count();

            foreach (var course in courses)
            {
                if (course.Modules != null)
                {
                    course.Modules = modules.Skip(i).Take(step).ToList();
                    i = i + step;
                }
            }

            // Add activities to modules

            i = 0;
            step = activities.Count() / modules.Count();
            foreach (var module in modules)
            {
                if (module.Activities != null)
                {
                    module.Activities = activities.Skip(step).Take(step).ToList();
                }
            }

            // Add teachers as users
            foreach (var user in teachers)
            {
                user.UserName = user.Email;
                IdentityResult result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Teacher").Wait();
                }
            }

            // Add students as users
            foreach (var user in students)
            {
                user.UserName = user.Email;
                IdentityResult result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Student").Wait();
                }
            }

            db.AddRange(activities);
            db.AddRange(modules);
            db.AddRange(courses);
            db.SaveChanges();
        }

        private static List<Course> GetCourses()
        {
            var fake = new Faker("sv");
            var courses = new List<Course>();
            for (int i = 0; i < 5; i++)
            {
                var course = new Course
                {
                    Title = fake.Company.CatchPhrase(),
                    StartDate = DateTime.Now.AddDays(fake.Random.Int(-2, 2)),
                };
                courses.Add(course);
            }
            return courses;
        }

        private static List<Module> GetModules()
        {
            var fake = new Faker("sv");
            var modules = new List<Module>();
            for (int i = 0; i < 15; i++)
            {
                var module = new Module
                {
                    Title = fake.Name.JobTitle(),
                    StartDate = fake.Date.Soon()
                };
                modules.Add(module);
            }
            return modules;
        }

        private static List<Activity> GetActivities()
        {
            var fake = new Faker("sv");
            var activitys = new List<Activity>();
            for (int i = 0; i < 45; i++)
            {
                var ran = fake.Random.Int(0, 4);
                var activity = new Activity
                {
                    Title = fake.Name.JobTitle(),
                    StartDate = fake.Date.Soon(),
                    Description = fake.Lorem.Sentence(),
                    ActivityType = GetActivityType(ran)
                };
                activitys.Add(activity);
            }
            return activitys;
        }

        private static List<ApplicationUser> GetTeachers()
        {
            var fake = new Faker("sv");

            var teachers = new List<ApplicationUser>();
            for (int i = 0; i < 20; i++)
            {
                string email = "";
                bool uniqueemail = false;

                while ((uniqueemail == false))
                {
                    email = fake.Internet.Email();
                    if (!teachers.Any(t => t.Email == email))
                    {
                        uniqueemail = true;
                    }
                };

                var teacher = new ApplicationUser
                {
                    Name = fake.Name.FullName(),
                    Email = email,
                };
                teachers.Add(teacher);
            }

            return teachers;
        }

        private static List<ApplicationUser> GetStudents()
        {
            var fake = new Faker("sv");

            var students = new List<ApplicationUser>();
            for (int i = 0; i < 25; i++)
            {
                string email = "";
                bool uniqueemail = false;

                while ((uniqueemail == false))
                {
                    email = fake.Internet.Email();
                    if (students.Count == 0 || !students.Any(t => t.Email == email))
                    {
                        uniqueemail = true;
                    }
                };

                var student = new ApplicationUser
                {
                    Name = fake.Name.FullName(),
                    Email = email,
                };
                students.Add(student);
            }

            return students;
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