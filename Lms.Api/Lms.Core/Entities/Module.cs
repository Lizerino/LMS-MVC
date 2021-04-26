using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lms.API.Core.Entities
{
    public class Module
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public int CourseId { get; set; }


    }

}