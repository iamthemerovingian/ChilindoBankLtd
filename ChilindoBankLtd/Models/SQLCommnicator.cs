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
                bool saveFailed = false;

                var account = context.BankAccounts
                                 .Where(a => a.AccountNumber.Equals(accountModel.AccountNumber))
                                 .FirstOrDefault();

                account.Balance += amount;
                do
                {
                    try
                    {
                        saveFailed = false;

                        context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        ex.Entries.Single().Reload();
                    }
                    catch (RetryLimitExceededException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine();
                    }
                } while (saveFailed);

                return account;
            }
        }

        public BankAccount Withdraw(BankAccountModel accountModel, decimal amount, string currency)
        {
            using (ChilindoBankLtdDB context = new ChilindoBankLtdDB())
            {
                bool saveFailed = false;

                var account = context.BankAccounts
                            .Where(a => a.AccountNumber.Equals(accountModel.AccountNumber))
                            .FirstOrDefault();

                account.Balance -= amount;

                do
                {
                    try
                    {
                        saveFailed = false;

                        context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        ex.Entries.Single().Reload();
                    }
                    catch (RetryLimitExceededException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine();
                    }
                } while (saveFailed);

                return account;
            }
        }
    }
}