using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.Core.ViewModels;
using Lms.MVC.Data.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Lms.MVC.Data.Data
{
    
public class MapperProfile: Profile
    {
        private readonly IUoW uow;


        public MapperProfile(IUoW uow)
        {
            this.uow = uow;

        }
        public MapperProfile()
        {

            CreateMap<ApplicationUser, ApplicationUsersListViewModel>()
                .ForMember(
                dest => dest.Email,
                from => from.MapFrom(u => u.Email))
                .ForMember(
                dest => dest.Role,
                from => from.MapFrom(u => GetRole(u))
                );
        }

        public string GetRole(ApplicationUser user)
        {
            var role = uow.UserRepository.GetRole(user);
            return role;
        }
    }    
}
