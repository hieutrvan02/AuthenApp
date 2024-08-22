using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenApp.Application.DTOs
{
    public class SuperPowerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateSuperPowerDto
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
