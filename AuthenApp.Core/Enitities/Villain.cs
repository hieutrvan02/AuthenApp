using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenApp.Core.Enitities
{
    public class Villain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EvilPlan { get; set; }

        // Many-to-many relationship to SuperHero
        public ICollection<SuperHeroVillain> SuperHeroVillains { get; set; }
    }
}
