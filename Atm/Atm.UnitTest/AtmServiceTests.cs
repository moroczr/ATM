namespace Atm.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Atm.Application.Interfaces;
    using Atm.Application.Models;
    using Atm.Application.Services;
    using Atm.Domain.Entities;
    using Atm.UnitTest.TestData;
    using NSubstitute;
    using Shouldly;
    using Xunit;

    /// <summary>
    /// Unit tests for AtmServices.
    /// </summary>
    public class AtmServiceTests
    {
        private readonly IRepository<MoneySlot> moneySlot;
        private readonly AtmService target;
        private readonly IAtmUnitOfWork uow;

        public AtmServiceTests()
        {
            this.moneySlot = Substitute.For<IRepository<MoneySlot>>();
            this.moneySlot.ReadAsListAsync().Returns(new List<MoneySlot>());

            this.uow = Substitute.For<IAtmUnitOfWork>();
            this.uow.MoneySlots.Returns(this.moneySlot);

            this.target = new AtmService(uow);
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(5000)]
        [InlineData(10000)]
        [InlineData(20000)]
        public Task WhenDepositAsyncIsCalledItAcceptsValidBankNotes(int bankNote)
        {
            var moneyUpload = new TransactionEntity[]
            {
                new TransactionEntity { BankNote = bankNote, Qty = 1 },
            };

            return Should.NotThrowAsync(this.target.DepositAsync(moneyUpload));
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(200)]
        public Task WhenDepositAsyncIsCalledItDoesNotAcceptsInvalidBankNotes(int bankNote)
        {
            var moneyUpload = new TransactionEntity[]
            {
                new TransactionEntity { BankNote = bankNote, Qty = 1 },
            };

            return Should.ThrowAsync<ArgumentException>(this.target.DepositAsync(moneyUpload));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-1000)]
        public Task WhenDepositAsyncIsCalledItDoesNotAcceptsZeroAndNegativeQuantities(int qty)
        {
            var moneyUpload = new TransactionEntity[]
            {
                new TransactionEntity { BankNote = 1000, Qty = qty },
            };

            return Should.ThrowAsync<ArgumentException>(this.target.DepositAsync(moneyUpload));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task WhenDepositIsCalledItReturnsSummaryOfAllSlots(bool startEmpty)
        {
            var moneyUpload = new TransactionEntity[]
            {
                new TransactionEntity { BankNote = 1000, Qty = 1 },
                new TransactionEntity { BankNote = 2000, Qty = 2 },
                new TransactionEntity { BankNote = 5000, Qty = 3 },
                new TransactionEntity { BankNote = 10000, Qty = 4 },
                new TransactionEntity { BankNote = 20000, Qty = 5 },
            };

            
            var testSlots = startEmpty ? new List<MoneySlot>() : GetTestMoneySlots();
            this.moneySlot.ReadAsListAsync().Returns(testSlots);
            

            var expectedSummary = moneyUpload.Sum(m => m.BankNote * m.Qty) + testSlots.Sum(m => m.BankNote * m.Qty);

            var result = await this.target.DepositAsync(moneyUpload);

            result.ShouldBe(expectedSummary);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-1000)]
        public Task WhenWithdrawAsyncIsCalledItDoesNotAcceptsZeroAndMinusQty(int qty)
        {
            return Should.ThrowAsync<ArgumentException>(this.target.WithdrawAsync(qty));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1001)]
        [InlineData(1200)]
        [InlineData(15003)]
        public Task WhenWithdrawAsyncIsCalledItDoesNotAcceptsNonThousands(int qty)
        {
            return Should.ThrowAsync<ArgumentException>(this.target.WithdrawAsync(qty));
        }

        [Fact]
        public Task WhenWithdrawIsCalledItThrowsExceptionIfAmountCantBeCovered()
        {
            this.moneySlot.ReadAsListAsync().Returns(GetTestMoneySlots()); // 160 000 in the slots

            return Should.ThrowAsync<ArgumentException>(this.target.WithdrawAsync(161000));
        }

        [Theory]
        [MemberData(nameof(WithdrawTestData.GetWithdrawResultTestData), MemberType = typeof(WithdrawTestData))]
        public async Task WhenWithdrawIsCalledItReturnsBankNotesAndQuantitiesCorrectly(int amount, IEnumerable<TransactionEntity> expectedResult)
        {
            this.moneySlot.ReadAsListAsync().Returns(GetTestMoneySlots());

            var result = await this.target.WithdrawAsync(amount);

            expectedResult.ShouldAllBe(e => ValidateItem(e, result));
        }

        private static bool ValidateItem(TransactionEntity expectedItem, IEnumerable<TransactionEntity> testResult)
        {
            var resultItem = testResult.First(r => r.BankNote.Equals(expectedItem.BankNote));
                resultItem.ShouldNotBeNull();
                resultItem.BankNote.ShouldBe(expectedItem.BankNote);
                resultItem.Qty.ShouldBe(expectedItem.Qty);

            return true;
        }

        private static List<MoneySlot> GetTestMoneySlots()
        {
            var slots = new List<MoneySlot>
            {
                new MoneySlot { BankNote = 1000, Qty = 1},
                new MoneySlot { BankNote = 2000, Qty = 2},
                new MoneySlot { BankNote = 5000, Qty = 3},
                new MoneySlot { BankNote = 10000, Qty = 4},
                new MoneySlot { BankNote = 20000, Qty = 5},
            };

            return slots;
        }

        
    }
}