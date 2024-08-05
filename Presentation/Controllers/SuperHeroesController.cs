using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenApp.Application.DTOs;
using AuthenApp.Infrastructure.Repositories;
using AuthenApp.Domain.Enitities;

namespace AuthenApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroesController : ControllerBase
    {
        private readonly ISuperHeroRepository _repository;
        private readonly IMapper _mapper;

        public SuperHeroesController(ISuperHeroRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHeroDto>>> GetAllHeroes()
        {
            var heroes = await _repository.GetAllHeroesAsync();
            var heroesDto = _mapper.Map<List<SuperHeroDto>>(heroes);
            return Ok(heroesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHeroDto>> GetHero(int id)
        {
            var hero = await _repository.GetHeroByIdAsync(id);
            if (hero == null)
            {
                return NotFound("Hero not found");
            }

            var heroDto = _mapper.Map<SuperHeroDto>(hero);
            return Ok(heroDto);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHeroDto>>> AddHero(CreateSuperHeroDto createSuperHeroDto)
        {
            var superHero = _mapper.Map<SuperHero>(createSuperHeroDto);
            await _repository.AddHeroAsync(superHero);
            var heroes = await _repository.GetAllHeroesAsync();
            var heroesDto = _mapper.Map<List<SuperHeroDto>>(heroes);
            return Ok(heroesDto);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<SuperHeroDto>> UpdateHero(SuperHeroDto superHeroDto)
        {
            var superHero = _mapper.Map<SuperHero>(superHeroDto);
            await _repository.UpdateHeroAsync(superHero);
            var updatedHero = await _repository.GetHeroByIdAsync(superHero.Id);
            var updatedHeroDto = _mapper.Map<SuperHeroDto>(updatedHero);
            return Ok(updatedHeroDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHeroDto>>> DeleteHero(int id)
        {
            await _repository.DeleteHeroAsync(id);
            var heroes = await _repository.GetAllHeroesAsync();
            var heroesDto = _mapper.Map<List<SuperHeroDto>>(heroes);
            return Ok(heroesDto);
        }
    }
}
