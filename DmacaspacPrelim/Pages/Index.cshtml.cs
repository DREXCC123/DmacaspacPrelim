using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using DmacaspacPrelim.Models;

namespace DmacaspacPrelim.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string? SelectedAction { get; set; }

        [BindProperty]
        public decimal Amount { get; set; }

        [BindProperty]
        public string? HolderName { get; set; }

        [BindProperty]
        public string? NewAccountNumber { get; set; }

        [BindProperty]
        public string? NewHolderName { get; set; }

        [BindProperty]
        public decimal NewBalance { get; set; }

        public List<BankAccount> Accounts { get; set; } = new List<BankAccount>();
        public BankAccount? CurrentAccount { get; set; }
        public string? Message { get; private set; }

        public void OnGet()
        {
            var accountsData = HttpContext.Session.GetString("Accounts");
            if (accountsData != null)
            {
                Accounts = System.Text.Json.JsonSerializer.Deserialize<List<BankAccount>>(accountsData);
            }

            
            if (Accounts.Count == 0)
            {
                Accounts.Add(new BankAccount("12345", "Drexler Macaspac", 1000));
                SaveAccountsToSession(Accounts);
            }
        }

        public void OnPost()
        {
            var accountsData = HttpContext.Session.GetString("Accounts");
            if (accountsData != null)
            {
                Accounts = System.Text.Json.JsonSerializer.Deserialize<List<BankAccount>>(accountsData);
            }

            if (!string.IsNullOrEmpty(NewAccountNumber) && !string.IsNullOrEmpty(NewHolderName))
            {
                Accounts.Add(new BankAccount(NewAccountNumber, NewHolderName, NewBalance));
                SaveAccountsToSession(Accounts);
                Message = "Account added successfully.";
                NewAccountNumber = null;
                NewHolderName = null;
                NewBalance = 0;
                return;
            }

            if (string.IsNullOrEmpty(HolderName))
            {
                Message = "Holder name must be provided.";
                return;
            }

            CurrentAccount = Accounts.Find(acc =>
                (string.IsNullOrEmpty(HolderName) || acc.HolderName == HolderName));

            if (CurrentAccount != null)
            {
                if (SelectedAction == "deposit")
                {
                    CurrentAccount.Deposit(Amount);
                    Message = $"Successfully deposited {Amount}. Current balance: {CurrentAccount.CurrentBalance}";
                }
                else if (SelectedAction == "withdraw")
                {
                    if (CurrentAccount.Withdraw(Amount))
                    {
                        Message = $"Successfully withdrew {Amount}. Current balance: {CurrentAccount.CurrentBalance}";
                    }
                    else
                    {
                        Message = "Insufficient funds.";
                    }
                }

                SaveAccountsToSession(Accounts);
            }
            else
            {
                Message = "Account not found.";
            }
        }

        private void SaveAccountsToSession(List<BankAccount> accounts)
        {
            var accountsData = System.Text.Json.JsonSerializer.Serialize(accounts);
            HttpContext.Session.SetString("Accounts", accountsData);
        }
    }
}
