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
using Lms.MVC.UI.Models.ViewModels;
using Lms.MVC.UI.Models.ViewModels.ActivityViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace Lms.MVC.UI.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly IUoW uow;

        private readonly IMapper mapper;

        public ActivitiesController(IMapper mapper, IUoW uow)
        {            
            this.mapper = mapper;
            this.uow = uow;
        }

        // GET: Activities
        public async Task<IActionResult> Index(int? Id)
        {
            if (Id != null)
            {
                var module = await uow.ModuleRepository.GetModuleAsync((int)Id);                

                var result = new ListActivityViewModel();
                result.ModuleId = (int)Id;
                result.ModuleTitle = module.Title;               

                return View(result);
            }
            else if (User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Modules");
            }            
                return RedirectToAction("Index","Courses");
        }

        // GET: Activities/Details/5
        [ModelValid]
        public async Task<IActionResult> Details(int? id)
        {
            var activity = await uow.ActivityRepository.GetActivityAsync(id);
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

                if (await uow.ActivityRepository.SaveAsync())
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
            var activity = await uow.ActivityRepository.GetActivityAsync(id);

            //create viewModel
            var model = mapper.Map<EditActivityViewModel>(activity);           
            return View(model);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValid]
        public async Task<IActionResult> Edit(int id, EditActivityViewModel activityModel)
        {
            var activity = await uow.ActivityRepository.GetActivityAsync(id);

            //activityModel.ModuleId = activity.ModuleId;
            mapper.Map(activityModel, activity);

            try
            {
                uow.ActivityRepository.Update(activity);
                await uow.ActivityRepository.SaveAsync();
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
            var activity = await uow.ActivityRepository.GetActivityAsync(id);
            return View(activity);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await uow.ActivityRepository.GetActivityAsync(id);
            uow.ActivityRepository.Remove(activity);
            await uow.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return uow.ActivityRepository.ActivityExists(id).Result;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetEvents(int Id)
        {
            var activities = await uow.ActivityRepository.GetAllActivitiesByModuleIdAsync(Id);
            var events = new List<SchedulerEvent>();
            foreach (var act in activities)
            {        
                var calevent = new SchedulerEvent();
                calevent.Id = act.Id;
                calevent.Title = act.Title;
                calevent.text = act.Description;
                calevent.start_date = act.StartDate;
                calevent.end_date = act.EndDate;
                events.Add(calevent);
            }
            var response = JsonConvert.SerializeObject(events);
            return Ok(response);
        }
    }
}