using AutoMapper;
using Lms.API.Data.Data;
using Lms.API.Data.Repositories;
using Lms.MVC.Core.Entities;
using Lms.MVC.Data.Data;
using Lms.MVC.UI.Extensions;
using Lms.MVC.UI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Lms.MVC.UI
{
    [Route("courses/{id}/modules")]
    public class ModuleWebController : Controller
    {
        private ApplicationDbContext db;
        private readonly IMapper mapper;

        public ModuleWebController(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        // GET: ModulerController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ModulerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //[Authorize(Roles = "Teacher, Admin")]
        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            return View();
        }

        //[Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(int id, Module module)//TODO: Configure API
        {
            if (ModelState.IsValid) 
            {
                //Find Course
                var course = db.Courses.FirstOrDefault(c => c.Id == id);//Todo Add Navigation to Course


                course.Modules = new List<Module>();

                //Add Module to Course
                course.Modules.Add(module);

                //Update Database
                await db.AddAsync(module);
                if (await db.SaveChangesAsync() ==1)
                {
                    
                    return View(course);
                }
                else
                {
                    return View();
                }               
            }
            return View();
        }

        //[Authorize(Roles = "Teacher, Admin")]
        [HttpGet]
        [Route("edit")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        //[Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit")]
        public async Task<ActionResult> Edit(int id, [Bind("Id, Title, Description, StartDate, EndDate")] ModuleDto moduleDto)
        {
            if (ModelState.IsValid)
            {

                var module = db.Modules.Find(id);

                try
                {
                    mapper.Map(moduleDto, module);


                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: ModulerController/Delete/5
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
    }
}
