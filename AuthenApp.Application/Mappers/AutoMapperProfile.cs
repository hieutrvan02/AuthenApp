using AutoMapper;
using AuthenApp.Application.DTOs;
using AuthenApp.Application.Enitities;

namespace AuthenApp.Application.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SuperHero, SuperHeroDto>().ReverseMap();
            CreateMap<SuperHero, CreateSuperHeroDto>().ReverseMap();
        }
    }
}
