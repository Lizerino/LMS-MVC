using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {                
        public string Name { get; set; }
        public string Role { get; set; }

        // If a student its the course the student takes. If a teacher its the courses the teacher teaches
        public ICollection<Course> Courses { get; set; }
        // nav prop
        public ICollection<Document> Documents { get; set; }
        

    }
}
