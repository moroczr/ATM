namespace Atm.UnitTest.TestData
{
    using System.Collections.Generic;
    using Atm.Application.Models;

    /// <summary>
    /// Test data for withdraw.
    /// </summary>
    public static class WithdrawTestData
    {
        public static IEnumerable<object[]> GetWithdrawResultTestData()
        {
            // result when requesting 160 000
            var fullAmount = new TransactionEntity[]
            {
                new TransactionEntity { BankNote = 1000, Qty = 1 },
                new TransactionEntity { BankNote = 2000, Qty = 2 },
                new TransactionEntity { BankNote = 5000, Qty = 3 },
                new TransactionEntity { BankNote = 10000, Qty = 4 },
                new TransactionEntity { BankNote = 20000, Qty = 5 },
            };

            // result when requesting 80 000
            var halfAmount = new TransactionEntity[]
            {
                new TransactionEntity { BankNote = 20000, Qty = 4 },
            };

            // result when requesting 16 000
            var tenPercent = new TransactionEntity[]
            {
                new TransactionEntity { BankNote = 10000, Qty = 1 },
                new TransactionEntity { BankNote = 5000, Qty = 1 },
                new TransactionEntity { BankNote = 1000, Qty = 1 },
            };

            yield return new object[] { 160000, fullAmount };
            yield return new object[] { 80000, halfAmount };
            yield return new object[] { 16000, tenPercent };
        }
    }
}