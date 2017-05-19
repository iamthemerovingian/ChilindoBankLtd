using ChilindoBankLtd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ChilindoBankLtd.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController: ApiController
    {
        SQLCommnicator sqlComm = new SQLCommnicator();
        ModelFactory modelFactory = new ModelFactory();

        [Route("balance")]
        public HttpResponseMessage Get(int accountnumber)
        {
            BankAccountModel result = modelFactory.Create(sqlComm.GetAccount(accountnumber));

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            
            return Request.CreateResponse(HttpStatusCode.OK, modelFactory.CreateResponse(result, message: "Bank Account fetch success."));
        }

        //Withdraw
        [Route("withdraw")]
        public HttpResponseMessage Get(int accountNumber, decimal amount, string currency)
        {
            BankAccountModel result = modelFactory.Create(sqlComm.GetAccount(accountNumber));

            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            if (amount < 0)
                return Request.CreateResponse(HttpStatusCode.Forbidden, modelFactory.CreateResponse(result, false, message: "Invalid Amount!"));

            if (!result.Currency.Equals(currency, StringComparison.OrdinalIgnoreCase))
                return Request.CreateResponse(HttpStatusCode.Conflict, modelFactory.CreateResponse(result, false, message: "Currency Mismatch"));

            if (result.IsLocked)
                return Request.CreateResponse(HttpStatusCode.RequestTimeout, modelFactory.CreateResponse(result, false, message: "Sorry for the inconvenience, the sever is busy, please try again later."));
   
            if (result.Balance < amount)
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Your account balance is insufficient to fulfill this request.");
            
            result = modelFactory.Create(sqlComm.Withdraw(result, amount, currency));
            return Request.CreateResponse(HttpStatusCode.OK, modelFactory.CreateResponse(result, message: "Withdrawal Complete!"));
        }

        //Deposit
        [Route("deposit")]
        public HttpResponseMessage Put(int accountNumber, decimal amount, string currency)
        {
            BankAccountModel result = modelFactory.Create(sqlComm.GetAccount(accountNumber));


            if (result == null )
                return Request.CreateResponse(HttpStatusCode.NotFound);

            if (amount < 0)
                return Request.CreateResponse(HttpStatusCode.Forbidden, modelFactory.CreateResponse(result, false, message: "Invalid Amount!"));

            if (!result.Currency.Equals(currency, StringComparison.OrdinalIgnoreCase))
                return Request.CreateResponse(HttpStatusCode.Conflict, modelFactory.CreateResponse(result, false, message: "Currency Mismatch"));

            if (result.IsLocked)
                return Request.CreateResponse(HttpStatusCode.RequestTimeout, modelFactory.CreateResponse(result,false, message: "Sorry for the inconvenience, the sever is busy, please try again later."));

            result = modelFactory.Create(sqlComm.Deposit(result, amount, currency));
            return Request.CreateResponse(HttpStatusCode.OK, modelFactory.CreateResponse(result, message: "Deposit Complete!"));
        }
    }
}