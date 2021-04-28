using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lms.MVC.Core.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        // nav prop
        public ICollection<Student> Students { get; set; }

        public ICollection<Module> Modules { get; set; }

        public ICollection<Teacher> Teachers { get; set; }

        public ICollection<Document> Documents { get; set; }
    }
}