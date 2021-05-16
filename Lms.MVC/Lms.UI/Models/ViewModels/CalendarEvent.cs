using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.UI.Models.ViewModels
{
  public class CalendarEvent
  {
    public string id { get; set; }

    public string title { get; set; }
    
    public string description { get; set; }

    
    public string start { get; set; }

    
    public string end { get; set; }

    
    public string typeid { get; set; }

        public string moduletitle { get; set; }
        public int moduleid { get; set; }
    }
}