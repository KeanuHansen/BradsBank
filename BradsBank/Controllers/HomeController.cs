using BradsBank.Models;
using BradsBank.Views.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BradsBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn(string username, string password)
        {
            int i = 1;
            int j = 2;
            int k = i + j;

            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult Register(string error = "none")
        {
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
