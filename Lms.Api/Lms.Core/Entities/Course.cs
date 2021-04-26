using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Core.Entities
{
   public class Course
    {
        public int Id { get; set; }
        /// <summary>
        /// The course title
        /// </summary>
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        /// <summary>
        /// The date that the course starts
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The course's modules
        /// </summary>

        // nav prop 
        public ICollection<Module> Modules { get; set; }
    }
}
