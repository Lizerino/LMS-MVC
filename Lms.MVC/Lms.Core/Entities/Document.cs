using System;

namespace Lms.MVC.Core.Entities
{
    public class Document
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

        public string Path { get; set; }

        // TODO: Remove before shipping
        // Uncommment if needed
        //public Course Course { get; set; }
        //public Module Module { get; set; }
        //public Activity Activity { get; set; }
        //public User User { get; set; }

        public int? CourseId { get; set; }

        public int? ModuleId { get; set; }

        public int? ActivityId { get; set; }

        public int UserId { get; set; }
    }
}