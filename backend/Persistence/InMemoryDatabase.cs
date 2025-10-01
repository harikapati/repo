using System.Collections.Generic;
using ATMApplication.Models;

namespace ATMApplication.Persistence
{
    public class InMemoryDatabase
    {
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}