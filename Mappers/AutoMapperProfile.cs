using AutoMapper;
using AuthenApp.DTOs;
using AuthenApp.Models;

namespace AuthenApp.Mappers
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
