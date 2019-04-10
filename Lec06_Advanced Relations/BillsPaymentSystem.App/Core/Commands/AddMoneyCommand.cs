namespace BillsPaymentSystem.App.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class AddMoneyCommand : ICommand
    {
        private readonly BillsPaymentSystemContext context;

        public AddMoneyCommand(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            var userId = int.Parse(args[0]);
            var money = decimal.Parse(args[1]);

            var user = this.context.Users
                .Include(p => p.PaymentMethods)
                .ThenInclude(a => a.BankAccount)
                .Include(p => p.PaymentMethods)
                .ThenInclude(c => c.CreditCard)
                .FirstOrDefault(x => x.UserId == userId);

            var card = user.PaymentMethods.FirstOrDefault(x => x.CreditCardId != null);
            var acc = user.PaymentMethods.FirstOrDefault(x => x.BankAccountId != null);

            if (card == null)
            {
                acc.BankAccount.Balance += money;
            }
            else
            {
                card.CreditCard.Limit += money;
            }

            int result = context.SaveChanges();
            string resultMessage = string.Empty;
            if (result > 0)
            {
                resultMessage = $"{money} for {user.FirstName} {user.LastName} successful added";
            }
            else
            {
                throw new ArgumentException($"money has not been added for {user.FirstName} {user.LastName}");
            }

            return resultMessage;
        }
    }
}