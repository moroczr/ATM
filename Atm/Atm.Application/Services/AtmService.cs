namespace Atm.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Atm.Application.Interfaces;
    using Atm.Application.Models;

    public class AtmService : IAtmService
    {
        private readonly IAtmUnitOfWork uow;

        public AtmService(IAtmUnitOfWork uow)
        {
            this.uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public Task<int> DepositAsync(IEnumerable<TransactionEntity> depositDetails)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TransactionEntity>> WithdrawAsync(int amount)
        {
            throw new NotImplementedException();
        }
    }
}