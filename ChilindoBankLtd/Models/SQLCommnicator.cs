using ChilindoBankLtd.Data.Database;
using ChilindoBankLtd.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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

                if (query != null)
                {
                    return query;
                }
            }
            return null;
        }

        public BankAccount Deposit(BankAccountModel accountModel, decimal amount, string currency)
        {
            using (ChilindoBankLtdDB context = new ChilindoBankLtdDB())
            {
                var account = context.BankAccounts
                            .Where(a => a.AccountNumber.Equals(accountModel.AccountNumber))
                            .FirstOrDefault();
                LockAccount(context, account);

                account.Balance += amount;
                account.IsLocked = false;
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
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
                context.Entry(account).State = EntityState.Modified;
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