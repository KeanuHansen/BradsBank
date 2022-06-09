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

        public IActionResult DepositMoney(string username, string account, string amount)
        {
            // Do logic to add money

            // Get the amount from the database
            // make query to get current amount in this account before deposit, set to current_amount
            double current_amount = 0;

            // Add it by amount passed in
            double new_amount = 0;
            new_amount = current_amount + double.Parse(amount);

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
