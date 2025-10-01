using System;

namespace ATMApplication.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public AccountType AccountType { get; set; }
    }
}