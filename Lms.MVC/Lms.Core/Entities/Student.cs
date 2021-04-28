using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Core.Entities
{
    public class Student : ApplicationUser
    {
        public Course Course { get; set; }

        // Cant have this net freaks out
        // public int CourseId { get; set; }

    }
}
