using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthenApp.Application.Enitities;
using AuthenApp.Core.Enitities;
using System.Reflection.Emit;

namespace AuthenApp.Application.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<SuperHero> SuperHeroes { get; set; }
        public DbSet<SuperPower> SuperPowers { get; set; }
        public DbSet<Villain> Villains { get; set; }
        public DbSet<SuperHeroVillain> SuperHeroVillains { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // SuperHero <-> Villain many-to-many
            builder.Entity<SuperHeroVillain>()
                .HasKey(sv => new { sv.SuperHeroId, sv.VillainId });

            builder.Entity<SuperHeroVillain>()
                .HasOne(sv => sv.SuperHero)
                .WithMany(s => s.SuperHeroVillains)
                .HasForeignKey(sv => sv.SuperHeroId);

            builder.Entity<SuperHeroVillain>()
                .HasOne(sv => sv.Villain)
                .WithMany(v => v.SuperHeroVillains)
                .HasForeignKey(sv => sv.VillainId);
        }
    }
}
