using AutoMapper;
using Lms.MVC.Core.Entities;
using Lms.MVC.Data.Data;
using Lms.MVC.UI.Filters;
using Lms.MVC.UI.Models.ViewModels;
using Lms.MVC.UI.Models.ViewModels.ActivityViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Controllers
{

    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public ActivitiesController(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        // GET: Activities
        public async Task<IActionResult> Index(int? Id)
        {
            if (Id != null)
            {
                var moduleTitle = db.Modules.Where(m => m.Id == Id).FirstOrDefault().Title;
                var activityViewModel = new ListActivityViewModel();
                activityViewModel.ActivityList = await db.Activities.Where(a => a.ModuleId == Id).ToListAsync();

                activityViewModel.ModuleId = (int)Id;
                activityViewModel.ModuleTitle = moduleTitle;

                return View(activityViewModel);
            }
            else
            {
                if (User.IsInRole("Student")) return RedirectToAction("Index", "Modules");
                return RedirectToAction("Index", "Courses");
            }
        }

        // GET: Activities/Details/5
        [ModelValid]
        public async Task<IActionResult> Details(int? id)
        {
            var activity = await db.Activities
                .Include(a => a.ActivityType)
                .FirstOrDefaultAsync(m => m.Id == id);
            var activityViewModel = mapper.Map<DetailActivityViewModel>(activity);
            return View(activityViewModel);
        }

        [Authorize(Roles = "Teacher,Admin")]
        // GET: Activities/Create
        public IActionResult Create(int Id)
        {
            var activityViewModel = new CreateActivityViewModel();
            activityViewModel.ModuleId = Id;
            activityViewModel.StartDate = DateTime.Now;
            activityViewModel.EndDate = activityViewModel.StartDate.AddDays(1);
            activityViewModel.ActivityTypes = new SelectList(db.ActivityTypes, nameof(ActivityType.Id), nameof(ActivityType.Name));
            return View(activityViewModel);
        }

        [Authorize(Roles = "Teacher,Admin")]
        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [ValidateAntiForgeryToken]
        [ModelNotNull, ModelValid]
        public async Task<IActionResult> Create(CreateActivityViewModel activityViewModel)
        {
            //Find Module
            var module = await db.Modules.Include(c => c.Activities).FirstOrDefaultAsync(c => c.Id == activityViewModel.ModuleId);

            // Map view model to model
            var activity = mapper.Map<Activity>(activityViewModel);

            //Add activity to module                
            module.Activities.Add(activity);

            if (await db.SaveChangesAsync() == 1)
            {
                // Send user back to list of modules for that course
                return RedirectToAction("Index", new { id = activityViewModel.ModuleId });
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        // GET: Activities/Edit/5
        [ModelNotNull, ModelValid]
        public async Task<IActionResult> Edit(int? id)
        {
            //find activity in database
            var activity = await db.Activities.FindAsync(id);

            //create viewModel
            
            var model = mapper.Map<EditActivityViewModel>(activity);

            ViewData["ActivityTypeId"] = new SelectList(db.ActivityTypes, "Id", "Id", activity.ActivityTypeId);
            return View(model);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValid]
        public async Task<IActionResult> Edit(int id, EditActivityViewModel activityModel)// TODO Finish This
        {
                var activity = mapper.Map<Activity>(activityModel);
            
            try
            {
                db.Update(activity);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(activity.Id))  return NotFound();
                else  throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Activities/Delete/5
        [ModelNotNull, ModelValid]
        public async Task<IActionResult> Delete(int? id)
        {
            var activity = await db.Activities
                .Include(a => a.ActivityType)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await db.Activities.FindAsync(id);
            db.Activities.Remove(activity);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return db.Activities.Any(e => e.Id == id);
        }
    }
}