using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Lms.MVC.Core.Entities;
using Lms.MVC.Data.Data;
using Lms.MVC.UI.Models.ViewModels.CourseViewModels;
using Lms.MVC.UI.Utilities.Pagination;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public CoursesController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            db = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        //from TEST
        public CoursesController()
        {
        }

        // GET: Courses
        public async Task<IActionResult> Index(string search, string sortOrder, int page)
        {            
            if (search != null)
            {
                page = 1;
            }            

            string showOnlyMyCourses = Request.Cookies["ShowOnlyMyCourses"];
            List<Course> courses;

            var currentUser = await userManager.GetUserAsync(User);            

            if (showOnlyMyCourses=="true")
            {
            courses = await db.Courses.Include(c => c.Users)
            .Where(c => (String.IsNullOrEmpty(search) || (c.Title.Contains(search))) && (c.Users.Contains(currentUser))).ToListAsync();
            }
            else
            {
            courses = await db.Courses.Include(c=>c.Users)
            .Where(c => String.IsNullOrEmpty(search) || (c.Title.Contains(search))).ToListAsync();
            }

            var result = mapper.Map<IEnumerable<ListCourseViewModel>>(courses);

            ViewData["CurrentFilter"] = search;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Title_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "StartDate" ? "StartDate_desc" : "StartDate";
            
            switch (sortOrder)
            {
                case "Title_desc":
                    result = result.OrderByDescending(s => s.Title);
                    break;
                case "StartDate":
                    result = result.OrderBy(s => s.StartDate);
                    break;
                case "StartDate_desc":
                    result = result.OrderByDescending(s => s.StartDate);
                    break;
                default:
                    result = result.OrderBy(s => s.Title);
                    break;
            }

            var paginatedResult = result.AsQueryable().GetPagination(page, 10);

            return View(paginatedResult);            
        }

        public IActionResult ToggleMyCourses()
        {
            string showOnlyMyCourses = Request.Cookies["ShowOnlyMyCourses"];

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(900);

            if (showOnlyMyCourses == "true")
            {
                Response.Cookies.Append("ShowOnlyMyCourses", "false", option);
            }
            else
            {
                Response.Cookies.Append("ShowOnlyMyCourses", "true", option);
            }

            return RedirectToAction("Index", "Courses");
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

            var course = await db.Courses.Include(c => c.Users)
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
            var courseViewModel = new CreateCourseViewModel();
            courseViewModel.StartDate = DateTime.Now;            
            return View(courseViewModel);
        }

        // POST: Courses/Create To protect from overposting attacks, enable the specific properties
        // you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                courseViewModel.Users = new List<ApplicationUser>();
                if (User.IsInRole("Teacher"))
                {
                    courseViewModel.Users.Add(userManager.FindByIdAsync(userManager.GetUserId(User)).Result);
                }

                var course = mapper.Map<Course>(courseViewModel);
                db.Add(course);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index");
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

        //[Authorize(Roles = "Teacher,Admin")]
        //public IActionResult AssignTeachers()
        //{
        //    return View();
        //}

        //[Authorize(Roles = "Teacher,Admin")]
        //public async Task<IActionResult> AssignTeachers(int id, Teacher teacher)
        //{
        //    Course course = await db.Courses.Include(c => c.Teachers).FirstOrDefaultAsync(c => c.Id == id);

        // if (course is null) { return NotFound(); }

        // if (course.Teachers is null) { course.Teachers = new List<Teacher>(); }

        // course.Teachers.Add(teacher);

        // db.Update(course); await db.SaveChangesAsync();

        //    return View();
        //}

        //[Authorize(Roles = "Teacher,Admin")]
        //public async Task<IActionResult> RemoveTeacher(int? id, string teacherId)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        // var course = await db.Courses .FirstOrDefaultAsync(m => m.Id == id); var teacher = await
        // db.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId); if (course == null) { return
        // NotFound(); }

        //    return View(teacher);
        //}

        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Teacher,Admin")]
        //public async Task<IActionResult> RemoveTeacher(int? courseId, string teacherId)
        //{
        //    if (courseId is null || string.IsNullOrEmpty(teacherId))
        //    {
        //        return BadRequest();
        //    }

        // Course course = await db.Courses.Include(c => c.Teachers).FirstOrDefaultAsync(c => c.Id
        // == courseId);

        // if (course.Teachers is null) { return BadRequest(); } Teacher teacher =
        // course.Teachers.FirstOrDefault(t => t.Id == teacherId);

        // if (!course.Teachers.Remove(teacher)) return NotFound();

        // db.Update(course); db.SaveChanges();

        //    return View();
        //}

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