using System.Diagnostics;

using Lms.MVC.Data.Data;
using Lms.MVC.UI.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lms.MVC.UI.Controllers
{
    public class HomeController : Controller
    {      
        public HomeController()
        {            
        }

        public IActionResult Index()
        {            
            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Courses");
            }
            else if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "ApplicationUsers");
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