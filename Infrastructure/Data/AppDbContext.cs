using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities;

namespace Portfolio.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Experience> Experiences { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<Education> Educations { get; set; } = null!;
        public DbSet<Certification> Certifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Note: Data is now loaded dynamically from seed_data.json via the DbInitializer 
            // in Program.cs to avoid hardcoding information in the application binary.
        }
    }
}
