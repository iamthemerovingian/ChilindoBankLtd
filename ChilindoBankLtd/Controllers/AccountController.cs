using ChilindoBankLtd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<HttpResponseMessage> Get(int accountnumber)
        {
            BankAccountModel result = await Task.FromResult(modelFactory.Create(sqlComm.GetAccount(accountnumber)));

            if (result == null)
                return await Task.FromResult(Request.CreateResponse(HttpStatusCode.NotFound));
            
            return await Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, modelFactory.CreateResponse(result, message: "Bank Account fetch success.")));
        }

        //Withdraw
        [Route("withdraw")]
        public async Task<HttpResponseMessage> Get(int accountNumber, decimal amount, string currency)
        {
            try
            {
                BankAccountModel result = await Task.FromResult(modelFactory.Create(sqlComm.GetAccount(accountNumber)));

                if (result == null)
                    return await Task.FromResult(Request.CreateResponse(HttpStatusCode.NotFound));

                if (amount < 0)
                    return await Task.FromResult(Request.CreateResponse(HttpStatusCode.Forbidden, modelFactory.CreateResponse(result, false, message: "Invalid Amount!")));

                if (!result.Currency.Equals(currency, StringComparison.OrdinalIgnoreCase))
                    return await Task.FromResult(Request.CreateResponse(HttpStatusCode.Conflict, modelFactory.CreateResponse(result, false, message: "Currency Mismatch")));

                if (result.Balance < amount)
                    return await Task.FromResult(Request.CreateResponse(HttpStatusCode.Forbidden, "Your account balance is insufficient to fulfill this request."));

                result = await Task.FromResult(modelFactory.Create(sqlComm.Withdraw(result, amount, currency)));
                return await Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, modelFactory.CreateResponse(result, message: "Withdrawal Complete!")));

            }
            catch (Exception ex)
            {
                Console.WriteLine();
                return await Task.FromResult(Request.CreateResponse(HttpStatusCode.ServiceUnavailable));
            }
        }

        //Deposit
        [Route("deposit")]
        public async Task<HttpResponseMessage> Put(int accountNumber, decimal amount, string currency)
        {
            try
            {
                BankAccountModel result = await Task.FromResult(modelFactory.Create(sqlComm.GetAccount(accountNumber)));


                if (result == null)
                    return await Task.FromResult(Request.CreateResponse(HttpStatusCode.NotFound));

                if (amount < 0)
                    return await Task.FromResult(Request.CreateResponse(HttpStatusCode.Forbidden, modelFactory.CreateResponse(result, false, message: "Invalid Amount!")));

                if (!result.Currency.Equals(currency, StringComparison.OrdinalIgnoreCase))
                    return await Task.FromResult(Request.CreateResponse(HttpStatusCode.Conflict, modelFactory.CreateResponse(result, false, message: "Currency Mismatch")));

                //return await Task.FromResult(Request.CreateResponse(HttpStatusCode.RequestTimeout, modelFactory.CreateResponse(result,false, message: "Sorry for the inconvenience, the sever is busy, please try again later.")));


                result = await Task.FromResult(modelFactory.Create(sqlComm.Deposit(result, amount, currency)));

                return await Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, modelFactory.CreateResponse(result, message: "Deposit Complete!")));

            }
            catch (Exception e)
            {
                Console.WriteLine();
                return await Task.FromResult(Request.CreateResponse(HttpStatusCode.ServiceUnavailable));
            }
        }
    }
}