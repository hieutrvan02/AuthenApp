using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenApp.Application.DTOs;
using AuthenApp.Application.Enitities;
using AuthenApp.Infrastructure.Repositories;

namespace AuthenApp.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroesController : ControllerBase
    {
        private readonly IRepository<SuperHero> _repository;
        private readonly IMapper _mapper;

        public SuperHeroesController(IRepository<SuperHero> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all SuperHeroes.
        /// </summary>
        /// <returns>A list of SuperHero DTOs.</returns>
        [Authorize(Policy = "Permissions.Products.View")]
        [HttpGet]
        public async Task<ActionResult<List<SuperHeroDto>>> GetAllHeroes()
        {
            var heroes = await _repository.GetAllAsync();
            var heroesDto = _mapper.Map<List<SuperHeroDto>>(heroes);
            return Ok(heroesDto);
        }

        /// <summary>
        /// Retrieves a specific SuperHero by its ID.
        /// </summary>
        /// <param name="id">The ID of the SuperHero to retrieve.</param>
        /// <returns>The SuperHero DTO if found; otherwise, a 404 Not Found response.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHeroDto>> GetHero(int id)
        {
            var hero = await _repository.GetByIdAsync(id);
            if (hero == null)
            {
                return NotFound("Hero not found");
            }

            var heroDto = _mapper.Map<SuperHeroDto>(hero);
            return Ok(heroDto);
        }

        /// <summary>
        /// Adds a new SuperHero.
        /// </summary>
        /// <param name="createSuperHeroDto">The DTO containing details of the SuperHero to add.</param>
        /// <returns>A list of all SuperHero DTOs after the addition.</returns>
        [Authorize(Roles = nameof(UserRoles.User))]
        [HttpPost]
        public async Task<ActionResult<List<SuperHeroDto>>> AddHero(CreateSuperHeroDto createSuperHeroDto)
        {
            var superHero = _mapper.Map<SuperHero>(createSuperHeroDto);
            await _repository.AddAsync(superHero);
            var heroes = await _repository.GetAllAsync();
            var heroesDto = _mapper.Map<List<SuperHeroDto>>(heroes);
            return Ok(heroesDto);
        }

        /// <summary>
        /// Updates an existing SuperHero.
        /// </summary>
        /// <param name="superHeroDto">The DTO containing updated details of the SuperHero.</param>
        /// <returns>The updated SuperHero DTO.</returns>
        [Authorize(Policy = "Permissions.Products.Edit")]
        [HttpPut]
        public async Task<ActionResult<SuperHeroDto>> UpdateHero(SuperHeroDto superHeroDto)
        {
            var superHero = _mapper.Map<SuperHero>(superHeroDto);
            await _repository.UpdateAsync(superHero);
            var updatedHero = await _repository.GetByIdAsync(superHero.Id);
            var updatedHeroDto = _mapper.Map<SuperHeroDto>(updatedHero);
            return Ok(updatedHeroDto);
        }

        /// <summary>
        /// Deletes a specific SuperHero by its ID.
        /// </summary>
        /// <param name="id">The ID of the SuperHero to delete.</param>
        /// <returns>A list of all remaining SuperHero DTOs.</returns>
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHeroDto>>> DeleteHero(int id)
        {
            await _repository.DeleteAsync(id);
            var heroes = await _repository.GetAllAsync();
            var heroesDto = _mapper.Map<List<SuperHeroDto>>(heroes);
            return Ok(heroesDto);
        }
    }
}
