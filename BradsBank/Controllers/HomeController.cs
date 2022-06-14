using BradsBank.Models;
using BradsBank.Views.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace BradsBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            this.configuration = config;
        }

        public string hashingin256(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public string GetSalt()
        {
            var listofChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            var gettingStringOfChars = new char[3];

            var randomVar = new Random();

            for (int i = 0; i < gettingStringOfChars.Length; i++)
            {
                gettingStringOfChars[i] = listofChars[randomVar.Next(listofChars.Length)];
            }

            var stringResult = new String(gettingStringOfChars);

            return stringResult;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn(string username, string password)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            SqlCommand db = new SqlCommand("SELECT count(*) FROM Users", connection);
            var all = (int)db.ExecuteScalar();

            connection.Close();

            // Query for the salt using the username

            // Add the salt

            // Hash the password
            string hashedPass = hashingin256(password);

            // Check the password against the database

            // If it is right, pass it in

            // If it is wrong, go back

            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult Register(string username, string password, string? error)
        {
            // Query to see if the user exists

            // If they do exist, return

            // If not, get a salt

            // Add it to the password

            // Hash the password

            // Insert the username, hashed password, salt

            return View(new RegisterModel(error));
        }

        public IActionResult ValidateRegistration(string username, string password, string confirmed)
        {
            if(password != confirmed)
            {
                string error = "passwords";

                return RedirectToAction("Register", "Home", error);

            }
            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult WithdrawMoney (string username, string accountFrom, double amount)
        {
            //Abdul start:
            //sql statement to get the balance in accountFrom and save it in accoutFrom
            double fromAmount  = 0;

            //check if the account we are drawing money from has enough funds
            if(fromAmount < amount)
            {
                Console.WriteLine("account does not have enough funds");
                return RedirectToAction("AccountActions", "Home", username);

            }

            //else, if the accoutFrom has enough money, give the client the money and subtract the amount from the balance of accountFrom
            fromAmount -= amount;

            //sql statement to send this new accoutFrom and update the balance avaliable on that specific account


             Console.WriteLine($"Withdrawal of ${amount} was successful");


            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult TransferMoney (string username, string accountFrom, string accountTo, double amount)
        {
            //Abdul started writing:
            //sql statement to get the balance in accountFrom and save it in accoutFrom
            double fromAmount  = 0;

            //check if the account we are drawing money from has enough funds
            if(fromAmount < amount)
            {
                Console.WriteLine("account does not have enough funds");
                return RedirectToAction("AccountActions", "Home", username);
            }

            //sql statement to get the amount on the second account (accountTo)
            double toAmount = 0;
            
            // these variables hold the new balances for both accounts after the transfer
            toAmount += amount;
            fromAmount -= amount;

            //Send the data of updated balances for both accounts using sql

             Console.WriteLine("Transfered successfully");

            //Abdul's code ends here
            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult DepositMoney(string username, string account, double amount)
        {

            // Abdul:
            
            // sql query to get the current balance in the accout passed in and safe it in a variable called balance
            // balance = ?

            // Do logic to add money

            // Get the amount from the database
            // make query to get current amount in this account before deposit, set to current_amount
            double current_amount = 0;

            //Abdul: should this be a doulbe? remember the rounding error that brad talked about?
            current_amount += amount;

            //Abdul: update the amount into the database

            // Add it by amount passed in
            double new_amount = 0;
            new_amount = current_amount + amount;

            // Make the query
            string sql = "";

            sql += String.Format("UPDATE accounts SET amount = {0}", new_amount);

            // Run the query
            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult AccountActions(string? username)
        {
            return View();
        }

        public IActionResult Deposit(string? username)
        {
            return View();
        }

        public IActionResult Withdraw(string? username)
        {
            return View();
        }

        public IActionResult Transfer(string? username)
        {
            return View();
        }

        public IActionResult Transactions()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
