using ChilindoBankLtd.Data.Entities;

namespace ChilindoBankLtd.Models
{
    public class ModelFactory
    {
        public BankAccountModel Create(BankAccount bankAccount)
        {
            if (bankAccount == null)
                return null;

            return new BankAccountModel
            {
                AccountNumber = bankAccount.AccountNumber,
                Balance = bankAccount.Balance,
                Currency = bankAccount.Currency,
                IsLocked = bankAccount.IsLocked
            };
        }

        public RequestResponse CreateResponse(BankAccountModel bankAccount, bool successful=true, string message="")
        {
            if (bankAccount == null)
                return null;

            return new RequestResponse
            {
                AccountNumber = bankAccount.AccountNumber,
                Successful = successful,
                Balance = bankAccount.Balance,
                Currency = bankAccount.Currency,
                Message = message
            };
        }
    }
}