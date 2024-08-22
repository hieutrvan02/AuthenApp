using AuthenApp.Application.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenApp.Core.Enitities
{
    public class SuperPower
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Relationship to SuperHero
        public int SuperHeroId { get; set; }
        public SuperHero SuperHero { get; set; }
    }
}
