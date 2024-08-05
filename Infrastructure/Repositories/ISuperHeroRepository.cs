using AuthenApp.Domain.Enitities;

namespace AuthenApp.Infrastructure.Repositories
{
    public interface ISuperHeroRepository
    {
        Task<List<SuperHero>> GetAllHeroesAsync();
        Task<SuperHero> GetHeroByIdAsync(int id);
        Task AddHeroAsync(SuperHero superHero);
        Task UpdateHeroAsync(SuperHero superHero);
        Task DeleteHeroAsync(int id);
    }
}
