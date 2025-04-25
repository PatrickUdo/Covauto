using Microsoft.EntityFrameworkCore;
using Covauto.Domain.Entities;

namespace Covauto.Domain.Data
{
    public class LeenAutoContext : DbContext
    {
        public LeenAutoContext(DbContextOptions<LeenAutoContext> options) : base(options) { }

        public DbSet<LeenAutoRit> LeenAutoRit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}