using Lms.MVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Models.ViewModels.ApplicationUserViewModels
{
  public class ListApplicationUsersViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Course> Courses { get; set; }


    }
}
