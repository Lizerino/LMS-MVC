﻿using Lms.MVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Models.ViewModels.ModelViewModels
{
    public class DetailModuleViewModel
    {
        
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }        

        public ICollection<Activity> Activities { get; set; }

        public List<Module> ModuleList { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; }
    }
}
