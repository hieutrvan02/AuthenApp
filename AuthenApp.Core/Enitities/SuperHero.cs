using AuthenApp.Core.Enitities;

namespace AuthenApp.Application.Enitities
{
    public class SuperHero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Origin { get; set; }

        // Relationships
        public ICollection<SuperPower> SuperPowers { get; set; }
        public ICollection<SuperHeroVillain> SuperHeroVillains { get; set; }
    }
}
