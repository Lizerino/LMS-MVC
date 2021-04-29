using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Lms.MVC.Core.Entities;
using Lms.MVC.Data.Data;
using Lms.MVC.UI.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Lms.MVC.UI.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IMapper mapper;

        public CoursesController(ApplicationDbContext context,  IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            db = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await db.Courses.ToListAsync();
            var result = mapper.Map<List<CourseListViewModel>>(courses);
            return View(result);
        }

        public async Task<IActionResult> RegisterForCourseToggle(int? id)
        {
            if (id == null) return NotFound();
            var course = await db.Courses.Include(m => m.Users).Where(i => i.Id == id).FirstOrDefaultAsync();
            var currentUser = await userManager.GetUserAsync(User);
            var teacher = userManager.Users.Include(x => x.Courses).Single(u => u == currentUser);


            if (course.Users.Contains(currentUser))
            {
                course.Users.Remove(currentUser);
                currentUser.Courses.Remove(course);
            }
            else
            {
                course.Users.Add(teacher);
                currentUser.Courses.Add(course);
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Courses");
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await db.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create To protect from overposting attacks, enable the specific properties
        // you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StartDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.Users = new List<ApplicationUser>();
                if (User.IsInRole("Teacher"))
                    course.Users.Add(userManager.FindByIdAsync(userManager.GetUserId(User)).Result);
                db.Add(course);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5 To protect from overposting attacks, enable the specific properties
        // you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartDate")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(course);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        public IActionResult AssignTeachers()
        {
            return View();
        }

        public async Task<IActionResult> AssignTeachers(int id, ApplicationUser teacher)
        {
            Course course = await db.Courses.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == id);

            if (course is null)
            {
                return NotFound();
            }

            if (course.Users is null)
            {
                course.Users = new List<ApplicationUser>();
            }

            course.Users.Add(teacher);

            db.Update(course);
            await db.SaveChangesAsync();

            return View(course);
        }

        public bool RemoveTeacher(int id, ApplicationUser teacher)
        {
            Course course = db.Courses.Include(c => c.Users).FirstOrDefault(c => c.Id == id);

            if (course is null)
            {
                return false;
            }

            if (course.Users is null)
            {
                return false;
            }

            if (!course.Users.Remove(teacher))
                return false;

            db.Update(course);
            db.SaveChanges();

            return true;
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await db.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await db.Courses.FindAsync(id);
            db.Courses.Remove(course);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return db.Courses.Any(e => e.Id == id);
        }
    }
}