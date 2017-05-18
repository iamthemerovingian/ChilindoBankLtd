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
    public class AccountController: ApiController
    {
        SQLCommnicator sqlComm = new SQLCommnicator();
        ModelFactory modelFactory = new ModelFactory();
        public HttpResponseMessage Get(int AccountNumber)
        {
            BankAccountModel result = modelFactory.Create(sqlComm.GetAccount(AccountNumber));

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, modelFactory.CreateResponse(result));
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}