using Lms.MVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Core.ViewModels
{
    class TeachersListViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string CourseTitle { get; set; }


    }
}
