using System;

namespace Lms.MVC.UI.Models.ViewModels
{
    public class SchedulerEvent
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string text { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_date { get; set; }

        public string ModuleType { get; set; }

        public string ModuleTitle { get; set; }

        public int ModuleId { get; set; }
    }
}