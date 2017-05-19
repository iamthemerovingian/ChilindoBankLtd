using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChilindoBankLtd.Controllers;
using ChilindoBankLtd.Data.Database;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ChilindoBankLtd.Models;
using ChilindoBankLtd.Data.Entities;
using System.Net;
using System.Web.Http.Routing;

namespace ChilindoBankLtd.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private AccountController controller;
        private ChilindoBankLtdDB dbcontext;
        private ModelFactory modelFactory;
        public AccountControllerTest()
        {
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            dbcontext = new ChilindoBankLtdDB();
            modelFactory = new ModelFactory();
        }

        [TestMethod]
        public void GetBalanceReturnsRequestResponse()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            HttpResponseMessage response = controller.Get(11111111);

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out RequestResponse bankAccount));
        }
        
        [TestMethod]
        public void WithdrawReturnsRequestResponse()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            HttpResponseMessage response = controller.Get(11111111, 1, "US");

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out RequestResponse bankAccount));
        }

        [TestMethod]
        public void DepositReturnsRequestResponse()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            HttpResponseMessage response = controller.Put(11111111, 1, "US");

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out RequestResponse bankAccount));
        }

        [TestMethod]
        public void GetBalanceReturnsCorrectAccount()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            HttpResponseMessage response = controller.Get(11111111);
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            response.TryGetContentValue(out RequestResponse bankAccount);
            Assert.AreEqual(dbBankAccount.AccountNumber, bankAccount.AccountNumber);
        }

        [TestMethod]
        public void WithdrawReducesAccountBalance()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = controller.Get(11111111, 1, "US");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out RequestResponse bankAccount));
            Assert.AreEqual(dbBankAccount.Balance - 1, bankAccount.Balance);
        }

        [TestMethod]
        public void DepositIncreasesBankAccountBalance()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = controller.Put(11111111, 1, "US");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out RequestResponse bankAccount));
            Assert.AreEqual(dbBankAccount.Balance + 1, bankAccount.Balance);
        }

        [TestMethod]
        public void BankBalanceCannotBeNegative()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = controller.Get(11111111, 99999999999999, "US");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public void CannotWithdrawInOtherCurrency()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = controller.Get(11111111, 1, "LK");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Conflict);
        }

        [TestMethod]
        public void CannotDepositInOtherCurrency()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = controller.Put(11111111, 1, "LK");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Conflict);
        }
    }
}
