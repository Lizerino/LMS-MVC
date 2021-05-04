using AutoMapper;
using Lms.API.Core.Entities;
using Lms.API.Core.Dto;
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
            CreateMap<Author, AuthorDto>().ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Now - src.BirthDate)).ReverseMap();
        }
    }
}
