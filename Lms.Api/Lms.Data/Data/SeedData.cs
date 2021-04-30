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

                if (db.Authors.Any())
                {
                    return;
                }

                var levels = new List<Level>();
                if (!db.Levels.Any())
                {
                    levels = GetLevels();
                    await db.AddRangeAsync(levels);
                }
                else
                    levels = await db.Levels.ToListAsync();

                var subjects = GetSubjects();

                var authors = GetAuthors();
                var literature = GetLiterature(authors, levels, subjects);
                //if (db.Courses.Any())
                //{
                //    return;
                //}

                //var modules = GetModules();
                //var courses = GetCourses();

                //for (int i = 0; i < 30; i += 3)
                //{
                //    var list = authors.Skip(i).ToList();
                //    foreach (var course in courses)
                //    {
                //        if (course.Modules != null)
                //        {
                //            continue;
                //        }
                //        course.Modules = list.Take(3).ToList();
                //        break;
                //    }
                //}
                await db.AddRangeAsync(authors);
                await db.AddRangeAsync(literature);

                await db.SaveChangesAsync();
            };
        }

        private static List<Literature> GetLiterature(List<Author> authors, List<Level> levels, List<Subject> subjects)
        {
            var fake = new Faker("sv");
            var literature = new List<Literature>();

            for(int i = 0; i < 40; i++)
            {
                var lit = new Literature
                {
                    Title = fake.Company.CompanyName(),
                    Description = fake.Lorem.Paragraph(),
                    ReleaseDate = fake.Date.Between(DateTime.Now.AddYears(-30),DateTime.Now.AddMonths(-3))
                };
                literature.Add(lit);
            }

            return literature;
        }

        private static List<Author> GetAuthors()
        {
            var fake = new Faker("sv");
            var authors = new List<Author>();
            for (int i = 0; i < 20; i++)
            {
                var author = new Author
                {
                    FirstName = fake.Name.FirstName(),
                    LastName = fake.Name.LastName(),
                    BirthDate = fake.Date.Between(DateTime.Today.AddYears(-100), DateTime.Today.AddYears(-20))
                };
                authors.Add(author);
            }
            return authors;
        }

        private static List<Subject> GetSubjects()
        {
            var fake = new Faker("sv");
            var subjects = new List<Subject>();
            for (int i = 0; i < 10; i++)
            {
                subjects.Add(new Subject
                {
                    Title = fake.Company.Bs()
                }); 
            }
            return subjects;
        }

        private static List<Level> GetLevels()
        {
            var levels = new List<Level>();

            levels.Add(new Level { Name = "Beginner" });
            levels.Add(new Level { Name = "Intermediate" });
            levels.Add(new Level { Name = "Advanced" });

            return levels;
        }
        //private static List<Module> GetModules()
        //{
        //    var fake = new Faker("sv");
        //    var modules = new List<Module>();
        //    for (int i = 0; i < 30; i++)
        //    {
        //        var module = new Module
        //        {
        //            Title = fake.Name.JobTitle(),
        //            StartDate = fake.Date.Soon()

        //        };
        //        modules.Add(module);
        //    }
        //    return modules;
        //}

        //private static List<Course> GetCourses()
        //{
        //    var fake = new Faker("sv");
        //    var courses = new List<Course>();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        var course = new Course
        //        {
        //            Title = fake.Company.CatchPhrase(),
        //            StartDate = DateTime.Now.AddDays(fake.Random.Int(-2, 2)),
        //        };
        //        courses.Add(course);
        //    }
        //    return courses;
        //}

    }
}
