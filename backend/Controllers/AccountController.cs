using Microsoft.AspNetCore.Mvc;
using ATMApplication.Models;
using ATMApplication.Services;
using ATMApplication.Persistence;

namespace ATMApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpGet("balance/{type}")]
        public IActionResult GetBalance(AccountType type)
        {
            var account = _service.GetAccount(type);
            if (account == null) return NotFound();
            return Ok(account.Balance);
        }

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] DepositRequest request)
        {
            try
            {
                _service.Deposit(request.Type, request.Amount);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawRequest request)
        {
            try
            {
                _service.Withdraw(request.Type, request.Amount);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferRequest request)
        {
            try
            {
                _service.Transfer(request.From, request.To, request.Amount);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("transactions")]
        public IActionResult GetTransactions()
        {
            // Return all transactions
            var db = (InMemoryDatabase)typeof(AccountService)
                .GetField("_db", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_service);
            return Ok(db.Transactions);
        }
    }

    public class DepositRequest
    {
        public AccountType Type { get; set; }
        public decimal Amount { get; set; }
    }

    public class WithdrawRequest
    {
        public AccountType Type { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransferRequest
    {
        public AccountType From { get; set; }
        public AccountType To { get; set; }
        public decimal Amount { get; set; }
    }
}