﻿using BradsBank.Models;
using BradsBank.Views.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System;
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

        public class User
        {
            public string name
            {
                get;
                set;
            }
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

        public IActionResult SignInValidation(string username, string password)
        {
            if(username != null)
            {
                // Query to see if the user exists
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();
                string checkUserExistsQuery = String.Format("SELECT COUNT(*) FROM Users WHERE Username = '{0}' ", username);
                SqlCommand db = new SqlCommand(checkUserExistsQuery, connection);
                var countUser = (int)db.ExecuteScalar();

                // If they do exist, return
                if (countUser == 0)
                {
                    connection.Close();

                    string user_name = "Username";
                    string go_To = string.Format("/Home/SignIn?error={0}", user_name);
                    return Redirect(go_To);
                }

                if (password == null || password == "")
                {
                    connection.Close();

                    string passwords = "Password";
                    string goTos = string.Format("/Home/SignIn?error={0}", passwords);
                    return Redirect(goTos);
                }

                // If not, get a salt
                string getSaltQuery = String.Format("SELECT Salt FROM Users WHERE Username = '{0}' ", username);
                db = new SqlCommand(getSaltQuery, connection);
                var getSalt = (string)db.ExecuteScalar();

                // Add it to the password
                var saltedPassword = password + getSalt;

                // Hash the password
                var hashedPassword = hashingin256(saltedPassword);

                // Insert the username, hashed password, salt
                string checkForUser = String.Format("SELECT COUNT(*) FROM Users WHERE Username = '{0}' AND HashedPassword = '{1}' ", username, hashedPassword);
                db = new SqlCommand(checkForUser, connection);
                var checkResult = (int)db.ExecuteScalar();

                if (checkResult > 0)
                {
                    // Close the database
                    connection.Close();

                    string goToo = string.Format("/home/accountactions?username={0}", username);
                    return Redirect(goToo);
                }
                else
                {
                    connection.Close();

                    string passwords = "Password";
                    string goTos = string.Format("/Home/SignIn?error={0}", passwords);
                    return Redirect(goTos);
                }
            }

            string user = "Username";
            string goTo = string.Format("/Home/SignIn?error={0}", user);
            return Redirect(goTo);
        }

        public IActionResult ValidateRegistration(string username, string password, string confirmed, string first, string last, string email)
        {
            if(username != null)
            {
                if (password != confirmed)
                {
                    string password_error = "MistmatchedPassword";
                    string go_To = string.Format("/Home/Register?error={0}", password_error);
                    return Redirect(go_To);
                }

                if (password == null || password == "" || confirmed == null || confirmed == "")
                {
                    string password_error = "MistmatchedPassword";
                    string go_To = string.Format("/Home/Register?error={0}", password_error);
                    return Redirect(go_To);
                }

                // Query to see if the user exists
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();
                string checkUserExistsQuery = String.Format("SELECT COUNT(*) FROM Users WHERE Username = '{0}' ", username);
                SqlCommand db = new SqlCommand(checkUserExistsQuery, connection);
                var countUser = (int)db.ExecuteScalar();

                // If they do exist, return
                if (countUser > 0)
                {
                    connection.Close();

                    string user_error = "UserExists";
                    string go_To2 = string.Format("/Home/Register?error={0}", user_error);
                    return Redirect(go_To2);
                }

                // If not, get a salt
                string getSalt = GetSalt();

                // Add it to the password
                var saltedPassword = password + getSalt;

                // Hash the password
                var hashedPassword = hashingin256(saltedPassword);

                // Insert the username, hashed password, salt
                string checkForUser = String.Format("INSERT INTO users (username, hashedpassword, salt, firstname, lastname, email) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}') ", username, hashedPassword, getSalt, first, last, email);
                db = new SqlCommand(checkForUser, connection);
                var tryit = db.ExecuteScalar();

                connection.Close();

                string goTo = string.Format("/home/accountactions?username={0}", username);
                return Redirect(goTo);
            }

            string final_error = "UserExists";
            string go_To3 = string.Format("/Home/Register?error={0}", final_error);
            return Redirect(go_To3);
        }

        public IActionResult Register(string? error = "none")
        {
            return View(new RegisterModel(error));
        }

        public IActionResult SignIn(string? error = "none")
        {
            return View(new SignInModel(error));
        }

        public IActionResult WithdrawMoney(string username, string accountFrom, double amount)
        {
            //Abdul start:


            //convert the amount into  pennies in order to store it into the database first

            amount *= 100;

            //withdraw cash query
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string getAccountNumber = String.Format("SELECT ACCOUNT FROM ACCOUNT WHERE USERNAME = '{0}' AND ACCOUNTTYPE = '{1}';", username, accountFrom);
            SqlCommand db = new SqlCommand(getAccountNumber, connection);
            var accountNumber = (Int16)db.ExecuteScalar();

            string withdrawQuery = String.Format("insert into Transactions (account, amount, transDesc) values ({0}, {1}, 'withdrawal')", accountNumber, -amount);
            db = new SqlCommand(withdrawQuery, connection);
            db.ExecuteNonQuery();

            //sql statement to send this new accoutFrom and update the balance avaliable on that specific account


            Console.WriteLine($"Withdrawal of ${amount} was successful");

            connection.Close();



            string goTo = string.Format("/home/accountactions?username={0}", username);
            return Redirect(goTo);
        }

        public IActionResult TransferMoney (string username, string accountFrom, string accountTo, double amount)
        {
            //Abdul started writing:

            //convert from dollars to pennies first
            amount *= 100;

            //withdraw cash query
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            string getAccountNumber = String.Format("SELECT ACCOUNT FROM ACCOUNT WHERE USERNAME = '{0}' AND ACCOUNTTYPE = '{1}';", username, accountFrom);
            SqlCommand db = new SqlCommand(getAccountNumber, connection);
            var accountNumberFrom = (Int16)db.ExecuteScalar();

            string getAccountNumberTo = String.Format("SELECT ACCOUNT FROM ACCOUNT WHERE USERNAME = '{0}' AND ACCOUNTTYPE = '{1}';", username, accountTo);
            db = new SqlCommand(getAccountNumberTo, connection);
            var accountNumberTo = (Int16)db.ExecuteScalar();

            string transQuery1 = String.Format("insert into Transactions (account, amount, transDesc) values ({0}, {1}, 'transfer from {2}')", accountNumberFrom, -amount, accountFrom);
            string transQuery2 = String.Format("insert into Transactions (account, amount, transDesc) values ({0}, {1}, 'transfer to {2}')", accountNumberTo, amount, accountTo);
            SqlCommand db1 = new SqlCommand(transQuery1, connection);
            SqlCommand db2 = new SqlCommand(transQuery2, connection);

            db1.ExecuteNonQuery();
            db2.ExecuteNonQuery();

            //string sql;
            //sql = "insert into Transactions (account, amount, tranDesc) values (account number, amount, transfer from accout no)";
            //sql = "insert into Transactions (account, amount, tranDesc) values (account number, -amount, transfer from accout no)";

            Console.WriteLine("Transfered successfully");

            connection.Close();



            //Abdul's code ends here
            string goTo = string.Format("/home/accountactions?username={0}", username);
            return Redirect(goTo);
        }

        public IActionResult DepositMoney(string username, string account, double amount)
        {
            //convert the amount from dollars into pennies
            amount *= 100;

            //withdraw cash query
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string getAccountNumber = String.Format("SELECT ACCOUNT FROM ACCOUNT WHERE USERNAME = '{0}' AND ACCOUNTTYPE = '{1}';", username, account);
            SqlCommand db = new SqlCommand(getAccountNumber, connection);
            var accountNumber = (Int16)db.ExecuteScalar();

            string depoQuery = String.Format("insert into Transactions (account, amount, transDesc) values ({0}, {1}, 'deposit')", accountNumber, amount);
            db = new SqlCommand(depoQuery, connection);
            db.ExecuteNonQuery();


            Console.WriteLine("Transfered successfully");

            connection.Close();

            string goTo = string.Format("/home/accountactions?username={0}", username);
            return Redirect(goTo);

        }

        public IActionResult TransactionsActions(string username)
        {
            if (username != null)
            {
                return View(new TransactionsActionsModel(username));
            }

            // Pass the variable into the model
            return View(new TransactionsActionsModel(username));
        }

        public IActionResult AccountActions(string username)
        {
            if (username != null)
            {
                return View(new AccountActionsModel(username));
            }

            // Pass the variable into the model
            return View(new AccountActionsModel(username));
        }

        public IActionResult Deposit(string? username)
        {
            if (username != null)
            {
                return View(new DepositModel(username));
            }

            return View(new DepositModel(username));
        }

        public IActionResult Withdraw(string? username)
        {
            if (username != null)
            {
                return View(new WithdrawModel(username));
            }

            return View(new WithdrawModel(username));
        }

        public IActionResult Transfer(string? username)
        {
            if (username != null)
            {
                return View(new TransferModel(username));
            }

            return View(new TransferModel(username));
        }

        public IActionResult Transactions(string? username, string? type = "none")
        {
            if (username != null)
            {
                return View(new TransactionsModel(username, type));
            }

            return View(new TransactionsModel(username, type));
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
