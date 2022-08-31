namespace Atm.Application.Interfaces
{
    using System.Threading.Tasks;
    using Atm.Domain.Entities;

    /// <summary>
    /// Unit of work interface to support persistence ignorance.
    /// </summary>
    public interface IAtmUnitOfWork
    {
        IRepository<MoneySlot> MoneySlots { get; }

        Task<int> CommitAsync();
    }
}