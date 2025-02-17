using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UpriseTask.Entities;

namespace UpriseTask.Data
{
    public class SolarPlantContext : IdentityDbContext<IdentityUser>
    {
        public SolarPlantContext(DbContextOptions<SolarPlantContext> options) : base(options){}

        public DbSet<SolarPlantEntity> SolarPlants { get; set; }
        public DbSet<SolarPlantProductionEntity> SolarPlantProductions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<SolarPlantEntity>()
                .HasMany(s => s.SolarPlantProductions)
                .WithOne(p => p.SolarPlant)
                .HasForeignKey(p => p.SolarPlantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
