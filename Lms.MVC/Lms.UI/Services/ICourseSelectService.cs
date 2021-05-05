using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Services
{
    public interface ICourseSelectService
    {
        Task<IEnumerable<SelectListItem>> GetTypeAsync();
    }
}