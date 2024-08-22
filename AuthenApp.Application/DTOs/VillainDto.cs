using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenApp.Application.DTOs
{
    public class VillainDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EvilPlan { get; set; }
    }

    public class CreateVillainDto
    {
        public required string Name { get; set; }
        public string EvilPlan { get; set; } = string.Empty;
    }
}
