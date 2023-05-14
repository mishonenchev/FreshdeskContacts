using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FreshdeskContacts.Models;
using FreshdeskContacts.Models.Dtos;

namespace FreshdeskContacts.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<GithubUserDTO, GithubUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<GithubUserDTO, FreshdeskContactDTO>()
                .ForMember(dest => dest.Email, opt =>
                    opt.MapFrom(src => string.IsNullOrEmpty(src.Email) ? $"{src.Login}@github.com" : src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Name) ? src.Login : src.Name));

            CreateMap<FreshdeskContactDTO, GithubUser>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
