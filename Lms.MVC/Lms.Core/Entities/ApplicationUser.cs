using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {        
        // Props for every user
        public string Name { get; set; }

        // Props for teachers
        public List<Course> CoursesUserTeaches { get; set; }

        // Props for students
        public Course CourseUserTakes { get; set; }

        // nav prop
        public ICollection<Document> Documents { get; set; }

    }
}
