using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lms.MVC.Core.Entities;
using Lms.MVC.Data;

namespace Lms.MVC.UI.Models.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Display(Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Display(Name = "For Module")]
        public int ModuleId { get; set ; }
        [Display(Name = "Module Title")]
        public string ModuleTitle { get; set; }
        [Display(Name = "Type Of Activity")]
        public int ActivityTypeId { get; set; }

        // nav prop
        public ActivityType ActivityType { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
