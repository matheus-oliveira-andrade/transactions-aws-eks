using Microsoft.EntityFrameworkCore;
using Movements.Infrastructure.Data.Models;

namespace Movements.Infrastructure.Data
{
    public class MovementsDbContext : DbContext
    {
        public virtual DbSet<MovementModel> Movements { get; set; }

        public MovementsDbContext()
        {
        }
        
        public MovementsDbContext(DbContextOptions<MovementsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovementModel>()
                .ToTable("Movement")
                .HasKey(x => x.Id);
        }
    }
}