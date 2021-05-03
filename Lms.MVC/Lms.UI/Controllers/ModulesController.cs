﻿using AutoMapper;
using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;
using Lms.MVC.UI.Models.DTO;
using Lms.MVC.UI.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.UI
{
    
    public class ModulesController : Controller
    {
        private ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly IUoW uow;
        private readonly UserManager<ApplicationUser> userManager;

        public ModulesController(ApplicationDbContext db, IMapper mapper, IUoW uow, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.mapper = mapper;
            this.uow = uow;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index(int Id)
        {
            // Todo: Should we make this a part of the entity?
            var courseTitle = db.Courses.Where(c => c.Id == Id).FirstOrDefault().Title;            

            //Student View of Modules for Course
            if (User.IsInRole("Student"))
            {
                var user = await userManager.GetUserAsync(User);

                var userCourse = db.Users.Include(u => u.Courses).Where(c => c.Id == user.Id)
                                         .Select(u => u.Courses.FirstOrDefault().Id)
                                         .FirstOrDefault();
                
                var modules = await db.Modules.Where(m => m.CourseId == userCourse).ToListAsync();

                var moduleViewModel = new ModuleViewModel();
                moduleViewModel.ModuleList = modules;

                moduleViewModel.CourseId = Id;
                moduleViewModel.CourseTitle = courseTitle;                
                
                return View(moduleViewModel);
            }
            else
            {
                var moduleViewModel = new ModuleViewModel();
                moduleViewModel.ModuleList = await db.Modules.Where(m => m.CourseId == Id).ToListAsync();

                moduleViewModel.CourseId = Id;
                moduleViewModel.CourseTitle = courseTitle;

                return View(moduleViewModel);
            }

            // TODO: Everyone execept students go to modules and activities via course list no??

            //if (User.IsInRole("Teacher"))
            //{
            //    var user = GetUserByName();
            //    var courses = db.Courses.Where(c => c.Id == courseId).ToList();
            //    var modules = new List<Module>();

            //    foreach(var course in courses)
            //    {
            //        var modulesInCourse = db.Modules.Where(m => m.CourseId == course.Id).ToList();
            //        modules.AddRange(modulesInCourse);
            //    }
            //    return View(modules);
            //}
            //if (User.IsInRole("Admin"))
            //{
            //    return View(await db.Modules.ToListAsync());
            //}
            //else return View();
        }

        private ApplicationUser GetUserByName()
        {
            return db.Users.FirstOrDefault(u => u.Name == User.Identity.Name);
        }


        // This is an unneccecary method.. the link should redirecto to activities
        [HttpGet]
        [Route("details/{title}")]//Todo Fix Navigation
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
        public ActionResult Create(int Id)        
        {
            var moduleViewModel = new ModuleViewModel();
            moduleViewModel.CourseId = Id;
            moduleViewModel.StartDate = DateTime.Now;
            moduleViewModel.EndDate = moduleViewModel.StartDate.AddDays(1);            
            return View(moduleViewModel);
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new")]
        public async Task<IActionResult> Create(ModuleViewModel moduleViewModel)//TODO: Configure API
        {
            if (ModelState.IsValid) 
            {
                //Find Course
                var course = await db.Courses.Include(c=>c.Modules).FirstOrDefaultAsync(c => c.Id == moduleViewModel.CourseId);

                // Map view model to model
                var module = mapper.Map<Module>(moduleViewModel);

                //Add Module to Course                
                course.Modules.Add(module);
                
                if (await db.SaveChangesAsync() == 1)
                {
                    // Send user back to list of modules for that course
                    return RedirectToAction("Index", new { id=moduleViewModel.CourseId});
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
            ModuleDto model = new ModuleDto()
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
        public async Task<ActionResult> Edit(int id, [Bind("Id, Title, Description, StartDate, EndDate")] ModuleDto moduleDto)
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