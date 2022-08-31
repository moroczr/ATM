namespace Atm.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Atm.Application.Interfaces;
    using Atm.Application.Models;
    using Atm.Domain.Entities;

    public class AtmService : IAtmService
    {
        private readonly IAtmUnitOfWork uow;

        public AtmService(IAtmUnitOfWork uow)
        {
            this.uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public async Task<int> DepositAsync(IEnumerable<TransactionEntity> depositDetails)
        {
            ValidateDeposit(depositDetails);

            var uploadSlots = GetNormalizedSlots(depositDetails);

            var currentSlots = await uow.MoneySlots.ReadAsListAsync();
            
            
            ModifySlots(uploadSlots, currentSlots);

            await uow.CommitAsync();
            return currentSlots.Sum(c => c.BankNote * c.Qty);
        }

        private static void ModifySlots(IEnumerable<MoneySlot> uploadSlots, List<MoneySlot> currentSlots)
        {
            var existingBankNotes = currentSlots.Select(c => c.BankNote);
            var existingSlots = currentSlots.Where(c => existingBankNotes.Contains(c.BankNote));
            

            foreach (var item in existingSlots)
            {
                item.Qty += uploadSlots.FirstOrDefault(u => u.BankNote.Equals(item.BankNote))?.Qty ?? 0;
            }

            var missingSlots = uploadSlots
                .Where(d => !existingBankNotes.Contains(d.BankNote))
                .Select(d => new MoneySlot { BankNote = d.BankNote, Qty = d.Qty });


            currentSlots.AddRange(missingSlots);
        }

        private static IEnumerable<MoneySlot> GetNormalizedSlots(IEnumerable<TransactionEntity> depositDetails) => depositDetails
            .GroupBy(d => d.BankNote)
            .Select(d => new MoneySlot { BankNote = d.Key, Qty = d.Sum(s => s.Qty) });
        

        public async Task<IEnumerable<TransactionEntity>> WithdrawAsync(int amount)
        {
            ValidateWithdraw(amount);

            var currentSlots = await uow.MoneySlots.ReadAsListAsync();

            var withdrawResult = new List<TransactionEntity>();

            foreach (var item in currentSlots.OrderByDescending(c => c.BankNote))
            {
                amount = PullItemsFromSlot(amount, withdrawResult, item);
            }

            if (amount > 0)
            {
                throw new ArgumentException("Amount can't be covered", nameof(amount));
            }

            this.CleanUpSlots(currentSlots);

            await uow.CommitAsync();
            return withdrawResult;
        }

        private void CleanUpSlots(List<MoneySlot> currentSlots)
        {
            var zeroQtyItems = currentSlots.Where(c => c.Qty.Equals(0));

            uow.MoneySlots.DeleteRange(zeroQtyItems);
        }

        private static int PullItemsFromSlot(int amount, List<TransactionEntity> withdrawResult, MoneySlot item)
        {
            var pullFromSlotQty = Math.Min((int)Math.Floor((double)amount / item.BankNote), item.Qty);

            if (pullFromSlotQty > 0)
            {
                item.Qty -= pullFromSlotQty;
                amount -= pullFromSlotQty * item.BankNote;
                withdrawResult.Add(new TransactionEntity { BankNote = item.BankNote, Qty = pullFromSlotQty });
            }

            return amount;
        }


        private static void ValidateDeposit(IEnumerable<TransactionEntity> depositDetails)
        {
            var validAmounts = new int[] { 1000, 2000, 5000, 10000, 20000 };            

            // Check if request contains invalid bank note.
            if (depositDetails.Any(d => !validAmounts.Contains(d.BankNote)))
            {
                throw new ArgumentException("Invalid banknote in deposit request.", nameof(depositDetails));
            }
                
            // Check if request contains invalid qty.
            if (depositDetails.Any(d => d.Qty <= 0))
            {
                throw new ArgumentException("Invalid qty in deposit request.", nameof(depositDetails));
            }
        }

        private static void ValidateWithdraw(int amount)
        {
            if (amount <= 0 || (amount % 1000 > 0))
            {
                throw new ArgumentException("Invalid amount specified", nameof(amount));
            }
        }
    }
}