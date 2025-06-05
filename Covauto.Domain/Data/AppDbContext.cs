using Microsoft.EntityFrameworkCore;
using Covauto.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Covauto.Domain.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Auto> Autos { get; set; }
        public DbSet<LeenAutoReservering> LeenAutoReserveringen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeenAutoReservering>()
                .HasOne(r => r.Werknemer)
                .WithMany(u => u.LeenAutoReserveringen)
                .HasForeignKey(r => r.WerknemerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeenAutoReservering>()
                .HasOne(r => r.Auto)
                .WithMany(a => a.Reserveringen)
                .HasForeignKey(r => r.AutoId);
        }

    }
}