using System;
using System.Linq;
using ATMApplication.Models;
using ATMApplication.Persistence;

namespace ATMApplication.Services
{
    public class AccountService
    {
        private readonly InMemoryDatabase _db;

        public AccountService(InMemoryDatabase db)
        {
            _db = db;
        }

        public Account GetAccount(AccountType type)
        {
            return _db.Accounts.FirstOrDefault(a => a.Type == type);
        }

        public void Deposit(AccountType type, decimal amount)
        {
            var account = GetAccount(type);
            if (account == null) throw new Exception("Account not found");
            if (amount <= 0) throw new ArgumentException("Deposit amount must be positive");
            account.Balance += amount;
            _db.Transactions.Add(new Transaction
            {
                Date = DateTime.Now,
                Description = $"Deposit to {type}",
                Amount = amount,
                AccountType = type
            });
        }

        public void Withdraw(AccountType type, decimal amount)
        {
            var account = GetAccount(type);
            if (account == null) throw new Exception("Account not found");
            if (account.Balance < amount) throw new InvalidOperationException("Insufficient funds");
            account.Balance -= amount;
            _db.Transactions.Add(new Transaction
            {
                Date = DateTime.Now,
                Description = $"Withdraw from {type}",
                Amount = -amount,
                AccountType = type
            });
        }

        public void Transfer(AccountType from, AccountType to, decimal amount)
        {
            Withdraw(from, amount);
            Deposit(to, amount);
            _db.Transactions.Add(new Transaction
            {
                Date = DateTime.Now,
                Description = $"Transfer {amount} from {from} to {to}",
                Amount = amount,
                AccountType = from
            });
        }
    }
}