using AutoMapper;

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
            CreateMap<Course, EditCourseViewModel>().ReverseMap();

            // Modules
            CreateMap<Module, CreateModuleViewModel>().ReverseMap();            
            CreateMap<Module, ListModuleViewModel>().ReverseMap();
            CreateMap<Module, EditModuleViewModel>().ReverseMap();

            // Activities
            CreateMap<Activity, CreateActivityViewModel>().ReverseMap();
            CreateMap<Activity, ListActivityViewModel>().ReverseMap();
            CreateMap<Activity, DetailActivityViewModel>().ReverseMap();
            CreateMap<Activity, EditActivityViewModel>().ReverseMap()
                .ForMember(a => a.ModuleId,
                opt => opt.Ignore());


            CreateMap<ApplicationUser, DetailsApplicationUserViewModel>();
            CreateMap<ApplicationUser, DeleteApplicationUserViewModel>().ReverseMap();


            CreateMap<ApplicationUser, ListApplicationUsersViewModel>()
              .ForMember(
              dest => dest.Email,
              from => from.MapFrom(u => u.Email))
              .ForMember(
              dest => dest.Role,
              opt => opt.MapFrom<RoleResolver>());

            CreateMap<ApplicationUser, EditApplicationUserViewModel>();
            //.ForMember(u => u.Courses, act => act.Ignore());
            CreateMap<EditApplicationUserViewModel, ApplicationUser>()
                                                                .ForMember(u => u.Courses, act => act.Ignore());
                                                                
        }
    }

    public class RoleResolver : IValueResolver<ApplicationUser, ListApplicationUsersViewModel, string>
    {

        private readonly IUoW uow;

        public RoleResolver(IUoW uow)
        {
            this.uow = uow;
        }
        public string Resolve(ApplicationUser source, ListApplicationUsersViewModel destination, string destMember, ResolutionContext context)
        {

            var role = uow.UserRepository.GetRole(source);

            return role;
        }
    }
}