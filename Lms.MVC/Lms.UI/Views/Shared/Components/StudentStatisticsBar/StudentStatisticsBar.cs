using Lms.MVC.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Views.Shared.Components.StudentStatisticsBar
{
    public class StudentStatisticsBar

    {
        public IEnumerable<string> LateAssignments { get; set; }
        public string NextDueAssignment { get; set; }
        public string CurrentMoudle { get; set; }
        public string NextModule { get; set; }
        public IEnumerable<string> Teachers { get; set; }
        public string CMAType { get; set; }



    }

    public class StudentStatisticsBarViewComponent : ViewComponent
    {
        private readonly IUoW uoW;
        public StudentStatisticsBarViewComponent(IUoW uoW)
        {
            this.uoW = uoW;
            
        }

        public IViewComponentResult Invoke(string cmaType, int? courseId, int? moduleId)
        {
            var bar = new StudentStatisticsBar();

            switch (cmaType.ToLower())
            {

                case "module" :
                    bar.LateAssignments = uoW.ActivityRepository.GetAllLateAssignmentsFromModuleAsync((int)moduleId).Result;
                    bar.CurrentMoudle = uoW.ModuleRepository.GetCurrentModule(courseId, moduleId);
                    bar.NextModule = uoW.ModuleRepository.GetNextModule(courseId, moduleId);
                    bar.NextDueAssignment = uoW.ActivityRepository.GetNextDueAssignment(courseId,moduleId);
                    bar.Teachers = uoW.CourseRepository.GetTeachersByModule(moduleId);

                    break;

                case "course":
                    bar.LateAssignments = uoW.ActivityRepository.GetAllLateAssignmentsFromCourseAsync((int)courseId);
                    bar.CurrentMoudle = uoW.ModuleRepository.GetCurrentModule(courseId, moduleId);
                    bar.NextModule = uoW.ModuleRepository.GetNextModule(courseId, moduleId);
                    bar.NextDueAssignment = uoW.ActivityRepository.GetNextDueAssignment(courseId, moduleId);
                    bar.Teachers = uoW.CourseRepository.GetTeachers(courseId);
                    break;
              
                default:
                     ModelState.AddModelError("", "Something went wrong");
                    break;
                    
            }
            bar.CMAType = cmaType;

            return View(bar);
        }
    }
}
