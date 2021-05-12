using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Itenso.TimePeriod;

using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;
using Lms.MVC.UI.Filters;
using Lms.MVC.UI.Models.ViewModels.ActivityViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lms.MVC.UI.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly IUoW uow;

        private readonly IMapper mapper;

        public ActivitiesController(ApplicationDbContext db, IMapper mapper, IUoW uow)
        {
            this.db = db;
            this.mapper = mapper;
            this.uow = uow;
        }

        // GET: Activities
        public async Task<IActionResult> Index(int? Id)
        {
            if (Id != null)
            {
                var moduleTitle = uow.ModuleRepository.GetModuleAsync((int)Id).Result.Title;

                //var moduleTitle = uow.ModuleRepository.GetModuleAsync((int)Id).Result.Title;

                //var moduleTitle = db.Modules.Where(m => m.Id == Id).FirstOrDefault().Title;
                var activityViewModel = new ListActivityViewModel();
                activityViewModel.ActivityList = await db.Activities.Where(a => a.ModuleId == Id).ToListAsync();

                activityViewModel.ModuleId = (int)Id;
                activityViewModel.ModuleTitle = moduleTitle;

                return View(activityViewModel);
            }
            else
            {
                if (User.IsInRole("Student")) return RedirectToAction("Index", "Modules");
                return RedirectToAction("Index");//, "Courses");
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
        public IActionResult Create(int Id)
        {
            var activityViewModel = new CreateActivityViewModel();
            activityViewModel.ModuleId = Id;
            activityViewModel.StartDate = DateTime.Now;
            activityViewModel.EndDate = activityViewModel.StartDate.AddDays(1);
            return View(activityViewModel);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelNotNull, ModelValid]
        public async Task<IActionResult> Create(CreateActivityViewModel activityViewModel)
        {
            //Find Module
            var modules = await uow.ModuleRepository.GetAllModulesAsync(true);
            var currentModule = modules.Where(c => c.Id == activityViewModel.ModuleId).FirstOrDefault();

            var activities = uow.ActivityRepository.GetAllActivitiesAsync().Result;
            var activitiesInCurrentModule = activities.Where(a => a.ModuleId == currentModule.Id);

            ValidateDates(activityViewModel, currentModule, activitiesInCurrentModule);

            if (ModelState.IsValid)
            {
                // Map view model to model
                var activity = mapper.Map<Activity>(activityViewModel);

                //Add activity to module
                currentModule.Activities.Add(activity);

                if (await db.SaveChangesAsync() == 1)
                {
                    // Send user back to list of activities for that module
                    return RedirectToAction("Index", "Activities", new { id = activityViewModel.ModuleId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(activityViewModel);
        }

        private void ValidateDates(CreateActivityViewModel activityViewModel, Module currentModule, IEnumerable<Activity> activitiesInCurrentModule)
        {
            TimePeriodCollection activitiesTimeperiod = new();
            TimeRange activityTimeRange = new(activityViewModel.StartDate, activityViewModel.EndDate);

            if (activitiesInCurrentModule.Count() > 0)
            {
                foreach (var item in activitiesInCurrentModule)
                {
                    activitiesTimeperiod.Add(new TimeRange(item.StartDate, item.EndDate));
                }
                if (activitiesTimeperiod.IntersectsWith(activityTimeRange))
                {
                    ModelState.AddModelError("", $"Dates overlap other activities in this module");
                }
            }

            if (activityViewModel.StartDate < currentModule.StartDate)
            {
                ModelState.AddModelError("StartDate", "Activity start date is before module start date");
            }
            if (activityViewModel.EndDate > currentModule.EndDate)
            {
                ModelState.AddModelError("EndDate", "Activity end date is after module end date");
            }
            if (activityViewModel.StartDate > activityViewModel.EndDate)
            {
                ModelState.AddModelError("EndDate", "An activity cannot end before it starts");
            }
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet]
        [ModelNotNull, ModelValid]
        public async Task<IActionResult> Edit(int? id)
        {
            //find activity in database
            var activity = await db.Activities.FindAsync(id);

            //create viewModel

            var model = mapper.Map<EditActivityViewModel>(activity);
            model.ActivityTypes = new SelectList(db.ActivityTypes, nameof(ActivityType.Id), nameof(ActivityType.Name));

            ViewData["ActivityTypeId"] = new SelectList(db.ActivityTypes, "Id", "Id", activity.ActivityTypeId);
            return View(model);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValid]
        public async Task<IActionResult> Edit(int id, EditActivityViewModel activityModel)
        {
            var activity = await db.Activities.FindAsync(id);

            //activityModel.ModuleId = activity.ModuleId;
            mapper.Map(activityModel, activity);

            try
            {
                db.Activities.Update(activity);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(activity.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction("Index", "Activities");
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet]
        [ModelNotNull, ModelValid]
        public async Task<IActionResult> Delete(int? id)
        {
            var activity = await db.Activities
                .Include(a => a.ActivityType)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(activity);
        }

        [Authorize(Roles = "Teacher,Admin")]
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