using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Lms.MVC.Core.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Lms.MVC.UI.Views.Shared.Components.UploadFile
{
    public class FileInfo
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string CMAType { get; set; }
    }
    public class UploadFileViewComponent : ViewComponent
    {        
        public IUoW uow { get; set; }        

        public UploadFileViewComponent(IUoW uow)
        {
            this.uow = uow;
        }

        public IViewComponentResult Invoke(string userId, int Id, string CMAType)
        {
            // userId is for the uploader
            // Id is for the course, module or activity where the file is uploaded
            FileInfo fileInfo = new FileInfo { Id=Id, userId=userId , CMAType=CMAType};           

            return View("UploadFile",fileInfo);
        }

    }
}
