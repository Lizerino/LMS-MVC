using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Models.ViewModels.AuthorViewModels
{
    public class IndexAuthorViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        
        public int Age { get; set; }
        public string OrderBy { get; set; }
    }
}
