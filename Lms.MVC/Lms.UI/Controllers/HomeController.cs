using System.Diagnostics;
using System.Linq;

using Lms.MVC.Data.Data;
using Lms.MVC.UI.Models;
using Lms.MVC.UI.Models.ViewModels.Admin;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lms.MVC.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.db = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Courses");
            }
            else if (User.IsInRole("Admin"))
            {
                var adminOverviewViewModel = new AdminOverviewViewModel();
                adminOverviewViewModel.NumberOfActivities = db.Activities.Count();                
                adminOverviewViewModel.NumberOfAdmins = db.Users.Where(u=>u.Role=="Admin").Count();
                adminOverviewViewModel.NumberOfCourses = db.Courses.Count();
                adminOverviewViewModel.NumberOfModules = db.Modules.Count();
                adminOverviewViewModel.NumberOfStudents = db.Users.Where(u => u.Role == "Student").Count();
                adminOverviewViewModel.NumberOfTeachers = db.Users.Where(u => u.Role == "Teacher").Count();
                adminOverviewViewModel.NumberOfUsers = db.Users.Count();
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