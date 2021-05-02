using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lms.MVC.Core.Entities;
using Lms.MVC.Data.Data;
using Lms.MVC.UI.Filters;
using Lms.MVC.UI.Models.ViewModels;
using AutoMapper;

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
            var activities = await db.Activities.Include(a => a.ActivityType).Where(a => a.ModuleId == Id).ToListAsync(); ;
            var result = mapper.Map<IEnumerable<ActivityViewModel>>(activities);
            
            var moduleTitle = db.Modules.Where(m => m.Id == Id).FirstOrDefault().Title;
            foreach (var activity in result)
            {
                activity.ModuleTitle = moduleTitle;
            }

            return View(result);
        }

        // GET: Activities/Details/5
        [ModelValidAndNotNull]
        public async Task<IActionResult> Details(int? id)
        {

            var activity = await db.Activities
                .Include(a => a.ActivityType)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["ActivityTypeId"] = new SelectList(db.ActivityTypes, "Id", "Id");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StartDate,EndDate,ModuleId,ActivityTypeId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Add(activity);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityTypeId"] = new SelectList(db.ActivityTypes, "Id", "Id", activity.ActivityTypeId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        [ModelValidAndNotNull]
        public async Task<IActionResult> Edit(int? id)
        {

            var activity = await db.Activities.FindAsync(id);

            ViewData["ActivityTypeId"] = new SelectList(db.ActivityTypes, "Id", "Id", activity.ActivityTypeId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartDate,EndDate,ModuleId,ActivityTypeId")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(activity);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityTypeId"] = new SelectList(db.ActivityTypes, "Id", "Id", activity.ActivityTypeId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        [ModelValidAndNotNull]
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
