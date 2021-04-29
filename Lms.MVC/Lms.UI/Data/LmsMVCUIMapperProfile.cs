using AutoMapper;

using Lms.MVC.Core.Entities;
using Lms.MVC.UI.Models.ViewModels;

namespace Lms.MVC.UI
{
    public class LmsMVCUIMapperProfile : Profile
    {
        public LmsMVCUIMapperProfile()
        {
            CreateMap<Course, CourseListViewModel>().ReverseMap();
        }
    }
}