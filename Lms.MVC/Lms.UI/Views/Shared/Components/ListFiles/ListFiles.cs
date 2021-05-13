using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lms.MVC.UI.Views.Shared.Components.ListFiles
{   

        [ViewComponent]
        public class ListFiles : ViewComponent
        {
            public ICollection<ApplicationFile> FileList { get; set; }

            public IUoW uow { get; set; }

            public ListFiles(IUoW uow)
            {
                this.uow = uow;
            }

            public IViewComponentResult Invoke(string UserCourseModuleActivity, string id)
            {                
                if (UserCourseModuleActivity.ToLower() == "user")
                {
                    FileList = uow.UserRepository.GetAllFilesByUserId(id).Result;
                }
                if (UserCourseModuleActivity.ToLower() == "course")
                {
                    FileList = uow.CourseRepository.GetAllFilesByCourseId(Int32.Parse(id)).Result;
                }
                if (UserCourseModuleActivity.ToLower() == "module")
                {
                    FileList = uow.ModuleRepository.GetAllFilesByModuleId(Int32.Parse(id)).Result;
                }
                if (UserCourseModuleActivity.ToLower() == "activity")
                {
                    FileList = uow.ActivityRepository.GetAllFilesByActivityId(Int32.Parse(id)).Result;
                }

                return View("ListFiles", FileList);
            }
        }
    }
