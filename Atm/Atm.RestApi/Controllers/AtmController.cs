namespace Atm.RestApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Atm.Application.Interfaces;
    using Atm.Application.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Atm controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api")]
    public class AtmController : ControllerBase
    {
        private readonly IAtmService atmService;
        private readonly ILogger<AtmController> logger;

        public AtmController(IAtmService atmService, ILogger<AtmController> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.atmService = atmService ?? throw new ArgumentNullException(nameof(atmService));
        }

        [HttpPost]
        [Route("withdrawal")]
        public async Task<Dictionary<int, int>> Withdraw(int amount)
        {
            this.logger.LogInformation($"Withdraw requested with amount {amount}");
            var result = await this.atmService.WithdrawAsync(amount);
            return result.ToDictionary(key => key.BankNote, value => value.Qty);
        }

        [HttpPost]
        [Route("deposit")]
        public Task<int> Deposit(Dictionary<int, int> request)
        {
            this.logger.LogInformation("Deposit requested");
            var payload = request.Select(c => new TransactionEntity { BankNote = c.Key, Qty = c.Value });
            return this.atmService.DepositAsync(payload);
        }
    }
}