using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ChilindoBankLtdClient.Models;
using System.Net.Http.Headers;

namespace ChilindoBankLtdClient
{
    public class Program
    {
        private static HttpClient client = new HttpClient();
        public static void Main(string[] args)
        {
            try
            {
                DoSomethingSimple().Wait();
                Main(new string[] { });
            }
            catch (Exception e)
            {
                Main(new string[] { });
            }
        }

        //Main Control Logic
        private static async Task DoSomethingSimple()
        {
            Console.WriteLine("State your request");

            var userInput = Console.ReadLine();

            var parameters = userInput.Split(',');

            var action = parameters.Length > 0 ? parameters[0] : "";
            var accountNumber = parameters.Length > 1 ? parameters[1] : "";
            var amount = parameters.Length > 2 ? parameters[2] : "";
            var currency = parameters.Length > 3 ? parameters[3] : "";

            if (action.Equals("balance", StringComparison.OrdinalIgnoreCase))
            {
                await GetBalance(accountNumber);
            }
            else if (action.Equals("deposit", StringComparison.OrdinalIgnoreCase))
            {
                await Deposit(accountNumber, amount, currency);
            }
            else if (action.Equals("withdraw", StringComparison.OrdinalIgnoreCase))
            {
                await Withdraw(accountNumber, amount, currency);
            }
            else
            {
                await DoRandomActions();
            }
        }
        private static async Task DoRandomActions()
        {
            Random randomGenerator = new Random();
            var limit = randomGenerator.Next(0, 1000);
            for (int i = 0; i < limit; i++)
            {
                var action = randomGenerator.Next(1, 3);
                var amount = randomGenerator.Next(0, 65535);

                if (action.Equals(1))
                {
                    await Deposit("11111111", amount.ToString(), "US");
                }

                if (action.Equals(2))
                {
                    await Withdraw("11111111", amount.ToString(), "US");
                }
            }
        }

        //Primary Functions.
        private static async Task Withdraw(string accountNumber, string amount, string currency)
        {
            var response = await client.GetAsync(GetWithdrawURL(int.Parse(accountNumber), decimal.Parse(amount), currency));
            await ProcessResponse(response);

        }
        private static async Task Deposit(string accountNumber, string amount, string currency)
        {
            var response = await client.PutAsJsonAsync(GetDepositURL(int.Parse(accountNumber), decimal.Parse(amount), currency), new RequestResponse() { });
            await ProcessResponse(response);
        }
        private async static Task GetBalance(string accountNumber)
        {
            var response = await client.GetAsync(GetBalanceURL(int.Parse(accountNumber)));
            await ProcessResponse(response);
        }

        //Building URIs.

        private static Uri GetWithdrawURL(int accountNumber, decimal amount, string currency)
        {
            var withdraw = new UriBuilder("http://localhost:55980/api/account/withdraw");
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["accountnumber"] = accountNumber.ToString();
            parameters["amount"] = amount.ToString();
            parameters["currency"] = currency.ToString();
            withdraw.Query = parameters.ToString();

            Uri finalUrl = withdraw.Uri;

            return finalUrl;
        }
        private static Uri GetDepositURL(int accountNumber, decimal amount, string currency)
        {
            var withdraw = new UriBuilder("http://localhost:55980/api/account/deposit");
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["accountnumber"] = accountNumber.ToString();
            parameters["amount"] = amount.ToString();
            parameters["currency"] = currency.ToString();
            withdraw.Query = parameters.ToString();

            Uri finalUrl = withdraw.Uri;

            return finalUrl;
        }
        private static Uri GetBalanceURL(int accountNumber)
        {
            var withdraw = new UriBuilder("http://localhost:55980/api/account/balance");
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["accountnumber"] = accountNumber.ToString();
            withdraw.Query = parameters.ToString();

            Uri finalUrl = withdraw.Uri;

            return finalUrl;
        }

        //Processing responses.
        private static async Task ProcessResponse(HttpResponseMessage response)
        {
            RequestResponse requestResponse = null;

            try
            {
                requestResponse = await response.Content.ReadAsAsync<RequestResponse>();
            }
            catch (Exception)
            {
            }

            PrintRequestResponse(requestResponse);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                PrintError(response);
            }
        }
        private static void PrintError(HttpResponseMessage response)
        {
            if (response == null)
            {
                return;
            }
            Console.WriteLine();
            Console.WriteLine("*----------------------------*");
            Console.WriteLine("Error: " + (int)response.StatusCode + " " + response.ReasonPhrase);
            Console.WriteLine("*----------------------------*");
            Console.WriteLine();

        }
        private static void PrintRequestResponse(RequestResponse requestResponse)
        {
            if (requestResponse == null)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Message: " + requestResponse.Message);
            Console.WriteLine("Account Number: " + requestResponse.AccountNumber);
            Console.WriteLine("Successful: " + requestResponse.Successful);
            Console.WriteLine("Balance: " + requestResponse.Balance);
            Console.WriteLine("Currency: " + requestResponse.Currency);
            Console.WriteLine();
        }
    }
}
