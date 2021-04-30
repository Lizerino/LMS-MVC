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

        //[ForeignKey("Course")]
        //public int CourseId { get; set; }

        // nav prop
        public ICollection<Document> Documents { get; set; }
        public ICollection<Course> Courses { get; set; }

    }
}
