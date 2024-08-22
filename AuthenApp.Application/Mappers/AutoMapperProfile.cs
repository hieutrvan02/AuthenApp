using AutoMapper;
using AuthenApp.Application.DTOs;
using AuthenApp.Application.Enitities;
using AuthenApp.Core.Enitities;

namespace AuthenApp.Application.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SuperHero, SuperHeroDto>().ReverseMap();
            CreateMap<SuperHero, CreateSuperHeroDto>().ReverseMap();
            CreateMap<Villain, VillainDto>().ReverseMap();
            CreateMap<Villain, CreateVillainDto>().ReverseMap();
        }
    }
}
