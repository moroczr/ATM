namespace Atm.Infrastructure.Persistence
{
    using System;
    using System.Threading.Tasks;
    using Atm.Application.Interfaces;
    using Atm.Domain.Entities;

    /// <summary>
    /// Unit of work implementation for the ATM data source.
    /// </summary>
    /// <seealso cref="Atm.Application.Interfaces.IAtmUnitOfWork" />
    public class AtmUnitOfWork : IAtmUnitOfWork
    {
        private readonly AtmDbContext ctx;

        public AtmUnitOfWork(AtmDbContext ctx, IRepository<MoneySlot> moneySlots)
        {
            this.ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            this.MoneySlots = moneySlots ?? throw new ArgumentNullException(nameof(moneySlots));
        }

        public IRepository<MoneySlot> MoneySlots { get; }

        public Task<int> CommitAsync()
        {
            return ctx.SaveChangesAsync();
        }
    }
}