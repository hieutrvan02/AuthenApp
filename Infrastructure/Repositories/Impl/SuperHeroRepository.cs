using Microsoft.EntityFrameworkCore;
using AuthenApp.Infrastructure.Repositories;
using AuthenApp.Infrastructure.Data;
using AuthenApp.Domain.Enitities;

namespace AuthenApp.Infrastructure.Repositories.Impl

{
    public class SuperHeroRepository : ISuperHeroRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SuperHeroRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SuperHero>> GetAllHeroesAsync()
        {
            return await _dbContext.SuperHeroes.ToListAsync();
        }

        public async Task<SuperHero> GetHeroByIdAsync(int id)
        {
            return await _dbContext.SuperHeroes.FindAsync(id);
        }

        public async Task AddHeroAsync(SuperHero superHero)
        {
            if (superHero == null)
            {
                throw new ArgumentNullException(nameof(superHero));
            }

            await _dbContext.SuperHeroes.AddAsync(superHero);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateHeroAsync(SuperHero superHero)
        {
            if (superHero == null)
            {
                throw new ArgumentNullException(nameof(superHero));
            }

            var dbHero = await GetHeroByIdAsync(superHero.Id);
            if (dbHero == null)
            {
                throw new KeyNotFoundException($"Hero with ID {superHero.Id} not found.");
            }

            _dbContext.Entry(dbHero).CurrentValues.SetValues(superHero);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteHeroAsync(int id)
        {
            var dbHero = await GetHeroByIdAsync(id);
            if (dbHero == null)
            {
                throw new KeyNotFoundException($"Hero with ID {id} not found.");
            }

            _dbContext.SuperHeroes.Remove(dbHero);
            await _dbContext.SaveChangesAsync();
        }
    }
}