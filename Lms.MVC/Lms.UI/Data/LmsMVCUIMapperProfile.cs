﻿using AutoMapper;

using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.UI.Models.ViewModels;
using Lms.MVC.UI.Models.ViewModels.ActivityViewModels;
using Lms.MVC.UI.Models.ViewModels.ApplicationUserViewModels;
using Lms.MVC.UI.Models.ViewModels.CourseViewModels;
using Lms.MVC.UI.Models.ViewModels.ModelViewModels;

namespace Lms.MVC.UI
{
    public class LmsMVCUIMapperProfile : Profile
    {
        public LmsMVCUIMapperProfile()
        {
            // Courses
            CreateMap<Course, CreateCourseViewModel>().ReverseMap();
            CreateMap<Course, ListCourseViewModel>().ReverseMap();

            // Modules
            CreateMap<Module, CreateModuleViewModel>().ReverseMap();            
            CreateMap<Module, ListModuleViewModel>().ReverseMap();

            // Activities
            CreateMap<Activity, CreateActivityViewModel>().ReverseMap();
            CreateMap<Activity, ListActivityViewModel>().ReverseMap();
            CreateMap<Activity, DetailActivityViewModel>().ReverseMap();


            CreateMap<ApplicationUser, ApplicationUsersListViewModel>()
              .ForMember(
              dest => dest.Email,
              from => from.MapFrom(u => u.Email))
              .ForMember(
              dest => dest.Role,
              opt => opt.MapFrom<RoleResolver>());

            CreateMap<ApplicationUser, ApplicationUserEditViewModel>();
            //.ForMember(u => u.Courses, act => act.Ignore());
            CreateMap<ApplicationUserEditViewModel, ApplicationUser>()
                                                                .ForMember(u => u.Courses, act => act.Ignore());
                                                                
        }
    }

    public class RoleResolver : IValueResolver<ApplicationUser, ApplicationUsersListViewModel, string>
    {

        private readonly IUoW uow;

        public RoleResolver(IUoW uow)
        {
            this.uow = uow;
        }
        public string Resolve(ApplicationUser source, ApplicationUsersListViewModel destination, string destMember, ResolutionContext context)
        {

            var role = uow.UserRepository.GetRole(source);

            return role;
        }
    }
}