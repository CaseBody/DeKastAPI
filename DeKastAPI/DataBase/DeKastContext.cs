using DeKastAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace DeKastAPI.DataBase
{
    public class DeKastContext : DbContext
    {
        public DeKastContext(DbContextOptions<DeKastContext> context) : base(context)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Abonnement>()
                .HasMany(x => x.AbonnementUses)
                .WithOne(x => x.Abonnement);
        }

        public DbSet<Abonnement> Abonnementen { get; set; }
        public DbSet<AbonnementUse> AbonnementUses { get; set; }
    }
}
