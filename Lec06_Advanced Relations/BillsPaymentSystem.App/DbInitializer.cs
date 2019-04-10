namespace BillsPaymentSystem.App
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data;
    using Models;
    using Models.Enums;

    public class DbInitializer
    {
        public static void Seed(BillsPaymentSystemContext context)
        {
            //user
            //payment
            //creditcard
            //bankaccount
            SeedUsers(context);

            //limit
            //moneyOwned
            //ExpDate
            SeedCreditCards(context);

            //balance
            //bankName
            //SWIFT
            SeedBankAccounts(context);

            //user
            //bankAccount
            //creditCard
            //payment method
            SeedPaymentMethod(context);
        }

        private static void SeedPaymentMethod(BillsPaymentSystemContext context)
        {
            var paymentMethods = new List<PaymentMethod>();

            for (int i = 0; i < 8; i++)
            {
                var paymentMethod = new PaymentMethod
                {
                    UserId = new Random().Next(1, 5),
                    PaymentType = (PaymentType) new Random().Next(1, 2),
                };

                if (i % 3 == 0)
                {
                    paymentMethod.CreditCardId = 1;
                    paymentMethod.BankAccountId = 1;
                }
                else if (i % 2 == 0)
                {
                    paymentMethod.CreditCardId = new Random().Next(1, 7);
                }
                else
                {
                    paymentMethod.BankAccountId = new Random().Next(1, 7);
                }

                if (IsValid(paymentMethod))
                {
                    paymentMethods.Add(paymentMethod);
                }
            }

            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges();
        }

        private static void SeedBankAccounts(BillsPaymentSystemContext context)
        {
            var balanceAmount = 30000;

            var bankAccounts = new List<BankAccount>();
            for (int i = 0; i < 8; i++)
            {
                var bankAccount = new BankAccount()
                {
                    Balance = i * (balanceAmount += i * 345),
                    BankName = "Bank of the Street No: " + i,
                    SWIFTCode = "SWIFT " + i + 13
                };
                //context.Entry(bankAccount).Property("Created").CurrentValue = DateTime.Now;
                //context.Entry(bankAccount).Property("Updated").CurrentValue = DateTime.Now;
                if (IsValid(bankAccount))
                {
                    bankAccounts.Add(bankAccount);
                }
            }

            context.AddRange(bankAccounts);

            context.SaveChanges();
        }

        private static void SeedCreditCards(BillsPaymentSystemContext context)
        {
            var creditCards = new List<CreditCard>();
            for (int i = 0; i < 8; i++)
            {
                var creditCard = new CreditCard
                {
                    Limit = new Random().Next(-5, 25000),
                    MoneyOwed = new Random().Next(-5, 20000),
                    ExpirationDate = DateTime.Now.AddDays(new Random().Next(-200, 200))
                };

                if (IsValid(creditCard))
                {
                    creditCards.Add(creditCard);
                }
            }

            context.CreditCards.AddRange(creditCards);
            context.SaveChanges();
        }

        private static void SeedUsers(BillsPaymentSystemContext context)
        {
            //first
            //last
            //email
            //password

            string[] firstNames = {"Gosho", "Pesho", "Mariq", "Neda", null, ""};
            string[] LastNames = {"Petrov", "Peshev", "Spasova", "Adena", null, "ERROR"};
            string[] emails = {"Petrov@abv.bg", "Peshev@abv.bg", "Spasova@abv.bg", "Adena@abv.bg", null, "ERROR"};
            string[] passwords =
                {"1234Petrov1234", "1234Peshev1234", "1234Spasova1234", "1234Adena1234", null, "ERROR"};

            List<User> users = new List<User>();

            for (int i = 0; i < firstNames.Length; i++)
            {
                var user = new User
                {
                    FirstName = firstNames[i],
                    LastName = LastNames[i],
                    Email = emails[i],
                    Password = passwords[i]
                };

                if (IsValid(user))
                {
                    users.Add(user);
                }
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

            return isValid;
        }
    }
}