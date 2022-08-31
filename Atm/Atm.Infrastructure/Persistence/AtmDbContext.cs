namespace Atm.Infrastructure.Persistence
{
    using Atm.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// DbContext for the Atm.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class AtmDbContext : DbContext
    {
        public AtmDbContext(DbContextOptions<AtmDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure qty did not changed while used in calculations and saved to database.
            modelBuilder.Entity<MoneySlot>()
            .Property(m => m.Qty).IsConcurrencyToken();
        }

        public DbSet<MoneySlot> MoneySlots { get; set; }
    }
}