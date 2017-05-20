using ChilindoBankLtd.Data.Database;
using ChilindoBankLtd.Data.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace ChilindoBankLtd.Models
{
    public class SQLCommnicator
    {
        public BankAccount GetAccount(int accountNumber)
        {
            using (ChilindoBankLtdDB context = new ChilindoBankLtdDB())
            {
                var query = context.BankAccounts
                            .Where(a => a.AccountNumber.Equals(accountNumber))
                            .FirstOrDefault();

                if (query == null)
                    return null;

                return query;
            }
        }

        public BankAccount Deposit(BankAccountModel accountModel, decimal amount, string currency)
        {
            using (ChilindoBankLtdDB context = new ChilindoBankLtdDB())
            {
                var  account = new BankAccount();
                bool safeFailed = false;
                safeFailed = false;

                account = context.BankAccounts
                                 .Where(a => a.AccountNumber.Equals(accountModel.AccountNumber))
                                 .FirstOrDefault();

                account.Balance += amount;
                context.Entry(account).State = EntityState.Modified;

                do
                {
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        safeFailed = true;

                        //context.Entry(account).Reload();
                        ex.Entries.Single().Reload();
                    }
                    catch (RetryLimitExceededException ex)
                    {
                        Console.WriteLine(ex);
                    }
                } while (safeFailed);

                return account;
            }
        }

        public BankAccount Withdraw(BankAccountModel accountModel, decimal amount, string currency)
        {
            using (ChilindoBankLtdDB context = new ChilindoBankLtdDB())
            {
                var account = context.BankAccounts
                            .Where(a => a.AccountNumber.Equals(accountModel.AccountNumber))
                            .FirstOrDefault();

                LockAccount(context, account);

                account.Balance -= amount;
                account.IsLocked = false;

                context.SaveChanges();
                return account;
            }
        }

        private static void LockAccount(ChilindoBankLtdDB context, BankAccount account)
        {
            account.IsLocked = true;
            context.Entry(account).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}