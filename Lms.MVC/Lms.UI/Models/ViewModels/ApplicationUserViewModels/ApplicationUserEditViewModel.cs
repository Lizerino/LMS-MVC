using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Models.ViewModels.ApplicationUserViewModels
{
    public class ApplicationUserEditViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [Compare("Email", ErrorMessage = "The Email and confirmation Email do not match.")]
        public string EmailConfirmed { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

    }
}
