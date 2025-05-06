using Microsoft.EntityFrameworkCore;
using Covauto.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Covauto.Domain.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LeenAutoRit> LeenAutoRit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}