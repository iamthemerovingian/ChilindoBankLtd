using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChilindoBankLtd.Data.Entities;

namespace ChilindoBankLtd.Models
{
    public class ModelFactory
    {
        public BankAccountModel Create(BankAccount bankAccount)
        {
            return new BankAccountModel
            {
                AccountNumber = bankAccount.AccountNumber,
                Balance = bankAccount.Balance,
                Currency = bankAccount.Currency,
                IsLocked = bankAccount.IsLocked
            };
        }

        public RequestResponse CreateResponse(BankAccountModel result, bool successful=true, string message="")
        {
            return new RequestResponse
            {
                AccountNumber = result.AccountNumber,
                Successful = successful,
                Balance = result.Balance,
                Currency = result.Currency,
                Message = message
            };
        }
    }
}