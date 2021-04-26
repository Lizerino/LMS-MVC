using Bogus;
using Lms.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Data.Data
{
    public class SeedData
    {
        public static async Task InitAsync(IServiceProvider services)
        {
            using (var db = new LmsAPIContext(services.GetRequiredService<DbContextOptions<LmsAPIContext>>()))
            {
                var fake = new Faker("sv");

                if (db.Courses.Any())
                {
                    return;
                }

                var modules = GetModules();
                var courses = GetCourses();

                for (int i = 0; i < 30; i += 3)
                {
                    var list = modules.Skip(i).ToList();
                    foreach (var course in courses)
                    {
                        if (course.Modules != null)
                        {
                            continue;
                        }
                        course.Modules = list.Take(3).ToList();
                        break;
                    }
                }
                await db.AddRangeAsync(courses);

                await db.SaveChangesAsync();
            };
        }
        private static List<Module> GetModules()
        {
            var fake = new Faker("sv");
            var modules = new List<Module>();
            for (int i = 0; i < 30; i++)
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

        private static List<Course> GetCourses()
        {
            var fake = new Faker("sv");
            var courses = new List<Course>();
            for (int i = 0; i < 10; i++)
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
       
    }
}
