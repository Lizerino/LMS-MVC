using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace Lms.MVC.UI.Views.Shared.Components.UploadFile
{
    public class UploadFileViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("UploadFile");
        }

    }
}
