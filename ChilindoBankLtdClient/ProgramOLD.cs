//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ChilindoBankLtdClient
//{
//    class ProgramOLD
//    {
//        private static async Task DoStuffAsync()
//        {
//            client.DefaultRequestHeaders.Accept.Clear();
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//            var balance = new UriBuilder("http://localhost:55980/api/account/balance");
//            var deposit = new UriBuilder("http://localhost:55980/api/account/deposit");



//            Console.WriteLine("What is your account Number?");
//            var accountNumber = int.Parse(Console.ReadLine());

//            var response = await client.GetAsync(GetBalanceURL(accountNumber));

//            if (response == null)
//            {
//                Console.WriteLine("That accoount does not Exist.");
//                await Task.Delay(1500);

//                Console.WriteLine("Restarting Process.");
//                await Task.Delay(1500);

//            }
//            var requestResponse = await response.Content.ReadAsAsync<RequestResponse>();


//            if (response.StatusCode != System.Net.HttpStatusCode.OK)
//            {
//                Console.WriteLine(response.StatusCode + " " + response.RequestMessage);
//                await Task.Delay(5000);
//            }

//            Console.WriteLine("Your Account Details are as Follows: ");
//            PrintRequestResponse(requestResponse);

//            Console.WriteLine("Please enter W for withdraw or D for deposit.");
//            var userResponse = Console.ReadLine();

//            if (userResponse.Equals("W", StringComparison.OrdinalIgnoreCase))
//            {
//                await Withdraw(accountNumber);
//            }
//            else
//            {
//                await Deposit(accountNumber);
//            }
//        }

//        private async static Task Deposit(int accountNumber)
//        {
//            Console.WriteLine("Please enter Amount to Deposit followed.");
//            var amount = Console.ReadLine();

//            Console.WriteLine("Please enter the currency code.");
//            var currency = Console.ReadLine();

//            var response = await client.PutAsJsonAsync(GetDepositURL(accountNumber, decimal.Parse(amount), currency), new RequestResponse() { });

//            if (response.StatusCode != System.Net.HttpStatusCode.OK)
//            {
//                Console.WriteLine(response.StatusCode + " " + response.RequestMessage);
//                await Task.Delay(5000);
//            }

//            var requestResponse = await response.Content.ReadAsAsync<RequestResponse>();
//            PrintRequestResponse(requestResponse);
//            Console.ReadLine();
//        }



//        private static async Task Withdraw(int accountNumber)
//        {
//            Console.WriteLine("Please enter Amount to Withdraw followed.");
//            var amount = Console.ReadLine();

//            Console.WriteLine("Please enter the currency code.");
//            var currency = Console.ReadLine();

//            var response = await client.GetAsync(GetWithdrawURL(accountNumber, decimal.Parse(amount), currency));

//            if (response.StatusCode != System.Net.HttpStatusCode.OK)
//            {
//                Console.WriteLine(response.StatusCode + " " + response.RequestMessage);
//                await Task.Delay(5000);
//            }

//            var requestResponse = await response.Content.ReadAsAsync<RequestResponse>();
//            PrintRequestResponse(requestResponse);
//            Console.ReadLine();
//        }
//    }
//}
