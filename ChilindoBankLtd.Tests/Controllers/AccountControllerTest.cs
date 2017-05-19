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
using Moq;
using System.Web.Http.Routing;

namespace ChilindoBankLtd.Tests.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTest
    /// </summary>
    [TestClass]
    public class AccountControllerTest
    {
        public AccountControllerTest()
        {
            //
            // TODO: Add constructor logic here
            //

            controller = new AccountController();
            dbcontext = new ChilindoBankLtdDB();
            modelFactory = new ModelFactory();
        }

        //private TestContext testContextInstance;
        private AccountController controller;
        private ChilindoBankLtdDB dbcontext;
        private ModelFactory modelFactory;

        ///// <summary>
        /////Gets or sets the test context which provides
        /////information about and functionality for the current test run.
        /////</summary>
        //public TestContext TestContext
        //{
        //    get
        //    {
        //        return testContextInstance;
        //    }
        //    set
        //    {
        //        testContextInstance = value;
        //    }
        //}

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetBalanceReturnsRequestResponse()
        {
            //Arrange
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.Get(11111111);

            // Assert
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue<RequestResponse>(out bankAccount));
        }
        
        [TestMethod]
        public void WithdrawReturnsRequestResponse()
        {
            //Arrange
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.Get(11111111, 1, "US");

            // Assert
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue<RequestResponse>(out bankAccount));
        }

        [TestMethod]
        public void DepositReturnsRequestResponse()
        {
            //Arrange
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.Put(11111111, 1, "US");

            // Assert
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue<RequestResponse>(out bankAccount));
        }

        [TestMethod]
        public void GetBalanceReturnsCorrectAccount()
        {
            //Arrange
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.Get(11111111);
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            RequestResponse bankAccount;
            response.TryGetContentValue<RequestResponse>(out bankAccount);
            Assert.AreEqual(dbBankAccount.AccountNumber, bankAccount.AccountNumber);
        }

        [TestMethod]
        public void WithdrawReducesAccountBalance()
        {
            //Arrange
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = controller.Get(11111111, 1, "US");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue<RequestResponse>(out bankAccount));
            Assert.AreEqual(dbBankAccount.Balance - 1, bankAccount.Balance);
        }

        [TestMethod]
        public void DepositIncreasesBankAccountBalance()
        {
            //Arrange
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = controller.Put(11111111, 1, "US");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            RequestResponse bankAccount;
            Assert.IsTrue(response.TryGetContentValue<RequestResponse>(out bankAccount));
            Assert.AreEqual(dbBankAccount.Balance + 1, bankAccount.Balance);
        }

        [TestMethod]
        public void BankBalanceCannotBeNegative()
        {
            //Arrange
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

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
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

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
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = controller.Put(11111111, 1, "LK");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Conflict);
        }

        [TestMethod]
        public void Cannot()
        {
            //Arrange
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            BankAccount dbResult = dbcontext.BankAccounts.Where(a => a.AccountNumber.Equals(11111111)).FirstOrDefault();
            HttpResponseMessage response = controller.Put(11111111, 1, "LK");
            BankAccountModel dbBankAccount = modelFactory.Create(dbResult);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Conflict);
        }

        [TestMethod]
        public void PostSetsLocationHeader_MockVersion()
        {
            // This version uses a mock UrlHelper.

            // Arrange
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://location/";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            controller.Url = mockUrlHelper.Object;

            // Act
            var response = controller.Put(11111111,1,"US");

            if (response.Headers.Location == null)
            {
                Assert.Fail();
            }

            if (locationUrl == null)
            {
                Assert.Fail();
            }

            // Assert
            Assert.AreEqual(locationUrl, response.Headers.Location.AbsoluteUri);
        }

        [TestMethod]
        public void DepositHasTheCorrectRoute()
        {
            // Arrange
            controller = new AccountController();

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/account")
            };

            controller.Configuration = new HttpConfiguration();

            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/account/deposit",
                defaults: new { controller });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "account" } });

            // Act
            var response = controller.Put(11111111, 1, "US");

            // Assert
            Assert.AreEqual("http://localhost/api/account", response.RequestMessage.RequestUri.AbsoluteUri);
        }
    }
}
