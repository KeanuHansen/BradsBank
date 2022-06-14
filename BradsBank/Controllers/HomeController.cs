using BradsBank.Models;
using BradsBank.Views.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using System.Drawing;
using System.Data.SqlClient;
using System.Text;
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

        public IActionResult WithdrawMoney (string username, string accountFrom, int amount)
        {
            //Abdul start:

            //make amount in pennies
            amount /=100;


            // create connection to the database
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=137.190.19.13;Initial Catalog=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            MessageBox.Show("Connection Open  !");
            cnn.Close();

            //sql statement to get the balance in accountFrom and save it in accoutFrom
            //string sql = "Select currentbalance from account where account = ACCOUNTNUMBER";

            //exec.sql

            double fromAmount  = 0 /100;


            sql = "insert into Transactions (account, amount, tranDesc) values (account number, amount, withdraw)";

            //sql statement to send this new accoutFrom and update the balance avaliable on that specific account


             Console.WriteLine($"Withdrawal of ${amount} was successful");


            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult TransferMoney (string username, string accountFrom, string accountTo, double amount)
        {
            //Abdul started writing:
            
             sql = "insert into Transactions (account, amount, tranDesc) values (account number, amount, transfer from accout no);
             sql = "insert into Transactions (account, amount, tranDesc) values (account number, -amount, transfer from accout no)";

             Console.WriteLine("Transfered successfully");

            //Abdul's code ends here
            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult DepositMoney(string username, string account, double amount)
        {

            // Abdul:
            
            //insert into the database

            sql = "insert into Transactions (account, amount, tranDesc) values (account number, amount, deposit)";


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
