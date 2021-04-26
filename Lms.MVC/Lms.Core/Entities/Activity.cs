using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Core.Entities
{
    class Activity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // nav prop
        public ActivityType ActivityType { get; set; }
        public ICollection<Document> Documents { get; set; }

    }
}
