using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Lms.MVC.Core.Entities;
using Lms.MVC.UI.Models.ViewModels;

namespace Lms.MVC.Data.Data
{
    
public class MapperProfileMVCUI: Profile
    {
        public MapperProfileMVCUI()
        {
            //CreateMap<sourceType, DestinationType>().ReverseMap();
            CreateMap<Course, CourseListViewModel>().ReverseMap();
        }
    }    
}
