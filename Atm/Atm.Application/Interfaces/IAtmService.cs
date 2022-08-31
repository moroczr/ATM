namespace Atm.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Atm.Application.Models;

    /// <summary>
    /// Interface for the atm services.
    /// </summary>
    public interface IAtmService
    {
        Task<IEnumerable<TransactionEntity>> WithdrawAsync(int amount);

        Task<int> DepositAsync(IEnumerable<TransactionEntity> depositDetails);
    }
}