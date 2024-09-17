namespace DmacaspacPrelim.Models
{
    public class BankAccount
    {
        public string AccountNumber { get; set; }
        public string HolderName { get; set; }
        public decimal CurrentBalance { get; set; }

        public BankAccount() { }

        public BankAccount(string accountNumber, string holderName, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            HolderName = holderName;
            CurrentBalance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                CurrentBalance += amount;
            }
        }

        public bool Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= CurrentBalance)
            {
                CurrentBalance -= amount;
                return true;
            }
            return false;
        }
    }
}
