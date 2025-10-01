using System;
using ATMApplication.Models;
using ATMApplication.Persistence;
using ATMApplication.Services;
using Xunit;

namespace ATMApplication.Tests
{
    public class AccountServiceTests
    {
        [Fact]
        public void Deposit_IncreasesBalance()
        {
            var db = new InMemoryDatabase();
            db.Accounts.Add(new Account { Id = 1, Type = AccountType.Checking, Balance = 0 });
            var service = new AccountService(db);
            service.Deposit(AccountType.Checking, 100);
            Assert.Equal(100, db.Accounts[0].Balance);
        }

        [Fact]
        public void Withdraw_DecreasesBalance()
        {
            var db = new InMemoryDatabase();
            db.Accounts.Add(new Account { Id = 1, Type = AccountType.Checking, Balance = 100 });
            var service = new AccountService(db);
            service.Withdraw(AccountType.Checking, 50);
            Assert.Equal(50, db.Accounts[0].Balance);
        }
            [Fact]
            public void Withdraw_InsufficientFunds_Throws()
            {
                var db = new InMemoryDatabase();
                db.Accounts.Add(new Account { Id = 1, Type = AccountType.Checking, Balance = 50 });
                var service = new AccountService(db);
                Assert.Throws<InvalidOperationException>(() => service.Withdraw(AccountType.Checking, 100));
            }

            [Fact]
            public void Deposit_NegativeAmount_Throws()
            {
                var db = new InMemoryDatabase();
                db.Accounts.Add(new Account { Id = 1, Type = AccountType.Checking, Balance = 0 });
                var service = new AccountService(db);
                Assert.Throws<ArgumentException>(() => service.Deposit(AccountType.Checking, -10));
            }

            [Fact]
            public void Transfer_InsufficientFunds_Throws()
            {
                var db = new InMemoryDatabase();
                db.Accounts.Add(new Account { Id = 1, Type = AccountType.Checking, Balance = 20 });
                db.Accounts.Add(new Account { Id = 2, Type = AccountType.Savings, Balance = 0 });
                var service = new AccountService(db);
                Assert.Throws<InvalidOperationException>(() => service.Transfer(AccountType.Checking, AccountType.Savings, 50));
            }

            [Fact]
            public void TransactionHistory_RecordsAllOperations()
            {
                var db = new InMemoryDatabase();
                db.Accounts.Add(new Account { Id = 1, Type = AccountType.Checking, Balance = 0 });
                db.Accounts.Add(new Account { Id = 2, Type = AccountType.Savings, Balance = 0 });
                var service = new AccountService(db);
                service.Deposit(AccountType.Checking, 100);
                service.Withdraw(AccountType.Checking, 40);
                service.Transfer(AccountType.Checking, AccountType.Savings, 30);
                var transactions = db.Transactions;
                Assert.Equal(5, transactions.Count);
                Assert.Contains("Deposit", transactions[0].Description); // Deposit to Checking
                Assert.Contains("Withdraw", transactions[1].Description); // Withdraw from Checking
                Assert.Contains("Withdraw", transactions[2].Description); // Withdraw from Checking (from Transfer)
                Assert.Contains("Deposit", transactions[3].Description); // Deposit to Savings (from Transfer)
                Assert.Contains("Transfer", transactions[4].Description); // Transfer record
                Assert.Equal(30, transactions[4].Amount);
            }

        [Fact]
        public void Transfer_MovesFundsBetweenAccounts()
        {
            var db = new InMemoryDatabase();
            db.Accounts.Add(new Account { Id = 1, Type = AccountType.Checking, Balance = 100 });
            db.Accounts.Add(new Account { Id = 2, Type = AccountType.Savings, Balance = 0 });
            var service = new AccountService(db);
            service.Transfer(AccountType.Checking, AccountType.Savings, 60);
            Assert.Equal(40, db.Accounts[0].Balance);
            Assert.Equal(60, db.Accounts[1].Balance);
        }
    }
}