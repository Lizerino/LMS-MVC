using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Models.ViewModels.AuthorViewModel
{
    public class IndexAuthorViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TimeSpan Age { get; set; }
        public string OrderBy { get; set; }
    }
}
