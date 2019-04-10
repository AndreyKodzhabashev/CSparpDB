namespace BillsPaymentSystem.App.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using Contracts;
    using Data;

    public class UserInfoCommand : ICommand
    {
        private readonly BillsPaymentSystemContext context;

        public UserInfoCommand(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            int userId = int.Parse(args[0]);

            var user = this.context.Users
                .Include(p => p.PaymentMethods)
                .ThenInclude(a => a.BankAccount)
                .Include(p => p.PaymentMethods)
                .ThenInclude(c => c.CreditCard)
                .FirstOrDefault(x => x.UserId == userId);

            if (user == null)
            {
                throw new ArgumentNullException("User not found!");
            }
            
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"User: {user.FirstName + " " + user.LastName}");
            sb.AppendLine("Bank Accounts");

            var bankAccounts = user.PaymentMethods.Where(x => x.BankAccountId != null).Select(x => x.BankAccount)
                .ToArray();
            foreach (var acc in bankAccounts)
            {
                sb.AppendLine($"-- ID: {acc.BankAccountId}");
                sb.AppendLine($"--- Balance: {acc.Balance}");
                sb.AppendLine($"--- Bank: {acc.BankName}");
                sb.AppendLine($"--- SWIFT: {acc.SWIFTCode}");
            }

            var creditCards = user.PaymentMethods.Where(x => x.CreditCardId != null).Select(x => x.CreditCard)
                .ToArray();
            sb.AppendLine("Credit Cards");
            foreach (var card in creditCards)
            {
                sb.AppendLine($"-- ID: {card.CreditCardId}");
                sb.AppendLine($"--- Limit: {card.Limit}");
                sb.AppendLine($"--- Money Owed: {card.MoneyOwed}");
                sb.AppendLine($"--- Limit Left: {card.LimitLeft}");
                sb.AppendLine($"--- Expiration Date: {card.ExpirationDate}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}