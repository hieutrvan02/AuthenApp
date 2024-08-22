namespace AuthenApp.Application.DTOs
{
    public class SuperHeroDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Origin { get; set; }

        // Relationships
        public ICollection<SuperPowerDto> SuperPowers { get; set; }
        public ICollection<SuperHeroVillainDto> SuperHeroVillains { get; set; }
    }

    public class CreateSuperHeroDto
    {
        public required string Name { get; set; }
        public string Alias { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
    }
}
