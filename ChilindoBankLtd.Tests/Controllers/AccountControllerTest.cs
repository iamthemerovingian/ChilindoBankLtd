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
        public void TestMethod1()
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
            Assert.IsTrue(response.TryGetContentValue<RequestResponse>(out bankAccount));
            Assert.AreEqual(dbBankAccount.AccountNumber,bankAccount.AccountNumber);
        }
    }
}
