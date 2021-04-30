using AutoMapper;
using Lms.MVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.MVC.Data.Data;

namespace Lms.API.Data.Data
{
   public class LmsAPIDataMapperProfile : Profile
    {

        public LmsAPIDataMapperProfile()
        {
            //CreateMap<Course, CourseDto>().ReverseMap();
            //CreateMap<Module, ModuleDto>().ReverseMap();
        }
    }
}
