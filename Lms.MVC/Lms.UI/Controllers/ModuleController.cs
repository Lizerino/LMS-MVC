using AutoMapper;
using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;
using Lms.MVC.UI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.UI
{
    //[Route("courses/{id}/modules/")]
    public class ModuleController : Controller
    {
        private ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly IUoW uow;
        private readonly UserManager<ApplicationUser> userManager;

        public ModuleController(ApplicationDbContext db, IMapper mapper, IUoW uow, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.mapper = mapper;
            this.uow = uow;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index(int id)
        {
            //if (User.IsInRole("Student"))
            //{
            //    var userName = User.Identity.Name;
            //    var user = db.Users.FirstOrDefault(u => u.Name == userName);
            //    var course = db.Courses.Find(user.CourseId);

            //    return View(await db.Modules.Where(m => m.CourseId == course.Id).ToListAsync());
            //}
            return View(await db.Modules.Where(m => m.CourseId == id).ToListAsync());
        }

        [HttpGet]
        [Route("details/{title}")]
        public ActionResult Details(int id, string title)
        {
            //Find course
            var course = db.Courses.Find(id);
            course.Modules = GetModules(course.Id);

            //Find module in course
            var module = course.Modules.FirstOrDefault(m => m.Title == title);

            //Add Activities
            module.Activities = GetActivities(module.Id);
            //mapper.Map<ModuleDto>(module);
            return View(module);
        }


        [Authorize(Roles = "Teacher, Admin")]
        [HttpGet]
        [Route("new")]
        public ActionResult Create()        
        {
            return View();
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new")]
        public async Task<IActionResult> Create(int id, Module module)//TODO: Configure API
        {
            if (ModelState.IsValid) 
            {
                //Find Course
                var course = db.Courses.FirstOrDefault(c => c.Id == id);



                //Add Module to Course
                course.Modules = new List<Module>();
                course.Modules.Add(module);

                //Unassign ID from module
                module.Id = 0;
                //Update Database
                 db.Modules.Add(module);
                if (await db.SaveChangesAsync() ==1)
                {

                    return RedirectToAction("Index", "Module");
                }
                else
                {
                    return View();
                }               
            }
            return View();
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpGet]
        [Route("edit/{title}")]
        public ActionResult Edit(string title)
        {
            //find and create display details of Module
            var module = db.Modules.FirstOrDefault(c => c.Title == title);
            ModuleViewModel model = new ModuleViewModel()
            {
                Id = module.Id,
                Title = module.Title,
                Description = module.Description,
                StartDate = module.StartDate,
                EndDate = module.EndDate
            };
            return View(model);
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{title}")]
        public async Task<ActionResult> Edit(int id, [Bind("Id, Title, Description, StartDate, EndDate")] ModuleViewModel moduleDto)
        {
            if (ModelState.IsValid)
            {
                //find module
                var module = db.Modules.Find(id);

                try
                {
                    //mapper.Map(moduleDto, module);
                    module.Title = moduleDto.Title;
                    module.Description = moduleDto.Description;
                    module.StartDate = moduleDto.StartDate;
                    module.EndDate = moduleDto.EndDate;
                    
                    db.Update(module);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: ModulerController/Delete/5
        [HttpGet]
        [Route("delete")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModulerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private List<Module> GetModules(int id)
        {
            var modules = new List<Module>();

            foreach(var module in db.Modules)
            {
                if(module.CourseId == id)
                {
                    modules.Add(module);
                }
            }
            return modules;
        }
        private List<Activity> GetActivities(int id)
        {
            var activities = new List<Activity>();

            foreach(var activity in db.Activities)
            {
                if(activity.ModuleId == id)
                {
                    activities.Add(activity);
                }
            }
                
            return activities;
        }
    }
}
