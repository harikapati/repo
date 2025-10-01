namespace ATMApplication.Models
{
    public enum AccountType
    {
        Checking,
        Savings
    }

    public class Account
    {
        public int Id { get; set; }
        public AccountType Type { get; set; }
        public decimal Balance { get; set; }
    }
}