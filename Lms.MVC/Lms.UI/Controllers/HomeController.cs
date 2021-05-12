using System.Diagnostics;
using System.Linq;

using Lms.MVC.Core.Repositories;
using Lms.MVC.UI.Models;
using Lms.MVC.UI.Models.ViewModels.Admin;

using Microsoft.AspNetCore.Mvc;

namespace Lms.MVC.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUoW uow;

        public HomeController(IUoW uow)
        {
            this.uow = uow;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Courses");
            }
            else if (User.IsInRole("Admin"))
            {
                var allUsers = uow.UserRepository.GetAllUsersAsync().Result;
                var adminOverviewViewModel = new AdminOverviewViewModel();
                adminOverviewViewModel.NumberOfCourses = uow.CourseRepository.GetAllCoursesAsync(false).Result.Count();
                adminOverviewViewModel.NumberOfModules = uow.ModuleRepository.GetAllModulesAsync(false).Result.Count();
                adminOverviewViewModel.NumberOfActivities = uow.ActivityRepository.GetAllActivitiesAsync().Result.Count();
                adminOverviewViewModel.NumberOfAdmins = allUsers.Where(u => u.Role == "Admin").Count();
                adminOverviewViewModel.NumberOfStudents = allUsers.Where(u => u.Role == "Student").Count();
                adminOverviewViewModel.NumberOfTeachers = allUsers.Where(u => u.Role == "Teacher").Count();
                adminOverviewViewModel.NumberOfUsers = allUsers.Count();
                return View("~/Views/AdminLanding/AdminOverview.cshtml", adminOverviewViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Modules");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}