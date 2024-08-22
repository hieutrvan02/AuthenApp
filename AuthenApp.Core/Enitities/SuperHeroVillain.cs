using AuthenApp.Application.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenApp.Core.Enitities
{
    public class SuperHeroVillain
    {
        public int SuperHeroId { get; set; }
        public SuperHero SuperHero { get; set; }

        public int VillainId { get; set; }
        public Villain Villain { get; set; }
    }
}
