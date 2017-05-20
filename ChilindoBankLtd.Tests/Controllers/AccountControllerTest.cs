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
using System.Threading.Tasks;

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
        public async Task GetBalanceReturnsRequestResponse()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            HttpResponseMessage response = await controller.Get(11111111);

            // Assert
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue(out bankAccount));
        }
        
        [TestMethod]
        public async Task WithdrawReturnsRequestResponse()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            HttpResponseMessage response = await controller.Get(11111111, 1, "US");

            // Assert
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue(out bankAccount));
        }

        [TestMethod]
        public async Task DepositReturnsRequestResponse()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            HttpResponseMessage response = await controller.Put(11111111, 1, "US");

            // Assert 
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue(out bankAccount));
        }

        [TestMethod]
        public async Task GetBalanceReturnsCorrectAccount()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            HttpResponseMessage response = await controller.Get(11111111);
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            RequestResponse bankAccount;
            response.TryGetContentValue(out bankAccount);
            Assert.AreEqual(dbBankAccount.AccountNumber, bankAccount.AccountNumber);
        }

        [TestMethod]
        public async Task WithdrawReducesAccountBalance()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = await controller.Get(11111111, 1, "US");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue(out bankAccount));
            Assert.AreEqual(dbBankAccount.Balance - 1, bankAccount.Balance);
        }

        [TestMethod]
        public async Task DepositIncreasesBankAccountBalance()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = await controller.Put(11111111, 1, "US");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue(out bankAccount));
            Assert.AreEqual(dbBankAccount.Balance + 1, bankAccount.Balance);
        }

        [TestMethod]
        public async Task BankBalanceCannotBeNegative()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = await controller.Get(11111111, decimal.MaxValue, "US");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task CannotWithdrawInOtherCurrency()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = await controller.Get(11111111, 1, "LK");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        }

        [TestMethod]
        public async Task CannotDepositInOtherCurrency()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = await controller.Put(11111111, 1, "LK");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        }
        [TestMethod]
        public async Task WillnotAcceptNegativeFiguresDeposit()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = await controller.Put(11111111, -1, "US");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert            
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue(out bankAccount));
            Assert.AreEqual(dbBankAccount.Balance, bankAccount.Balance);
        }
        [TestMethod]
        public async Task WillnotAcceptNegativeFiguresWithdrawal()
        {
            //Arrange
            controller = new AccountController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = await controller.Get(11111111, -1, "US");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert            
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue(out bankAccount));
            Assert.AreEqual(dbBankAccount.Balance, bankAccount.Balance);
        }

    }
}
