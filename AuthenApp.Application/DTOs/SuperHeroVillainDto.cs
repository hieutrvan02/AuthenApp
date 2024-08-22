using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenApp.Application.DTOs
{
    public class SuperHeroVillainDto
    {
        public int SuperHeroId { get; set; }
        public SuperHeroDto SuperHero { get; set; }

        public int VillainId { get; set; }
        public VillainDto Villain { get; set; }
    }

    public class CreateSuperHeroVillainDto
    {
        public required int SuperHeroId { get; set; }
        public required int VillainId { get; set; }
    }
}
