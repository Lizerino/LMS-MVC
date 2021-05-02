using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.UI.Models.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ModuleId { get; set; }
        public string ModuleTitle { get; set; }

        public List<Activity> ActivityList { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ActivityTypes { get; set; }

        // nav prop
        public ActivityType ActivityType { get; set; }
        public ICollection<Document> Documents { get; set; }

    }
}
