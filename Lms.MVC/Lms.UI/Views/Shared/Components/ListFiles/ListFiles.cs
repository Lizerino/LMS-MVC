using System;
using System.Collections.Generic;

using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Lms.MVC.UI.Views.Shared.Components.ListFiles
{
    public class ListFiles
    {
        public ICollection<ApplicationFile> FileList { get; set; }
    }

    public class ListFilesViewComponent : ViewComponent
    {
        public IUoW uow { get; set; }

        public ListFilesViewComponent(IUoW uow)
        {
            this.uow = uow;
        }

        public IViewComponentResult Invoke(string UserCourseModuleActivity, string id)
        {
            ListFiles files = new ListFiles();
            if (UserCourseModuleActivity.ToLower() == "user")
            {
                files.FileList = uow.UserRepository.GetAllFilesByUserId(id).Result;
            }
            if (UserCourseModuleActivity.ToLower() == "course")
            {
                files.FileList = uow.CourseRepository.GetAllFilesByCourseId(Int32.Parse(id)).Result;
            }
            if (UserCourseModuleActivity.ToLower() == "module")
            {
                files.FileList = uow.ModuleRepository.GetAllFilesByModuleId(Int32.Parse(id)).Result;
            }
            if (UserCourseModuleActivity.ToLower() == "activity")
            {
                files.FileList = uow.ActivityRepository.GetAllFilesByActivityId(Int32.Parse(id)).Result;
            }

            return View(files);
        }
    }
}