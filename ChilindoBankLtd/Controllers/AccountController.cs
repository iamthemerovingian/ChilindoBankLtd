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
    //[RoutePrefix("api/account")]
    public class AccountController: ApiController
    {
        SQLCommnicator sqlComm = new SQLCommnicator();
        ModelFactory modelFactory = new ModelFactory();

        //[Route("balance/{accountnumber}")]
        public HttpResponseMessage Get(int accountnumber)
        {
            BankAccountModel result = modelFactory.Create(sqlComm.GetAccount(accountnumber));

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, modelFactory.CreateResponse(result, message: "Bank Account fetch success."));
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        //Withdraw
        public HttpResponseMessage Get(int accountNumber, decimal amount, string currency)
        {
            BankAccountModel result = modelFactory.Create(sqlComm.GetAccount(accountNumber));

            if (result != null)
            {
                if (result.Balance >= amount && !result.IsLocked)
                {
                    result = modelFactory.Create(sqlComm.Withdraw(result, amount, currency));

                    return Request.CreateResponse(HttpStatusCode.OK, modelFactory.CreateResponse(result, message: "Withdrawal Complete!"));
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Your account balance is insufficient to fulfill this request.");
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        //Deposit
        public HttpResponseMessage Put(int accountNumber, decimal amount, string currency)
        {
            BankAccountModel result = modelFactory.Create(sqlComm.GetAccount(accountNumber));

            if (result != null && !result.IsLocked)
            {
                result = modelFactory.Create(sqlComm.Deposit(result, amount, currency));

                return Request.CreateResponse(HttpStatusCode.OK, modelFactory.CreateResponse(result, message: "Deposit Complete!"));
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}