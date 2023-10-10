using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Command> Commands { get; set; }

        /// <summary>
        /// This method is used to explicitly configure the entity mapping
        /// between Platform and Command entities
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Platform>()
            .HasMany(p => p.Commands)
            .WithOne(c => c.Platform)
            .HasForeignKey(c => c.PlatformId);

            modelBuilder.Entity<Command>()
            .HasOne(c => c.Platform)
            .WithMany(p => p.Commands)
            .HasForeignKey(c => c.PlatformId);
        }
    }
}