namespace Atm.RestApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using Atm.Application.Interfaces;
    using Atm.RestApi.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
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
        [Route("Withdraw")]
        public IEnumerable<TransactionModel> Withdraw(int amount)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("Deposit")]
        public int Deposit(IEnumerable<TransactionModel> depositDetails)
        {
            throw new NotImplementedException();
        }
    }
}