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

        public IActionResult SignInValidation(string username, string password)
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
                return RedirectToAction("SignIn", "Home", "Username");
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

                return RedirectToAction("AccountActions", "Home", username);
            }
            else
            {
                connection.Close();
                return RedirectToAction("SignIn", "Home", "Password");
            }
        }

        public IActionResult ValidateRegistration(string username, string password, string confirmed, string first, string last, string email)
        {

           // WithdrawMoney("mike", 123, )
            if(password != confirmed)
            {
                return RedirectToAction("Register", "Home", "MistmatchedPassword");
            }

            // Query to see if the user exists
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            string checkUserExistsQuery = String.Format("SELECT COUNT(*) FROM Users WHERE Username = '{0}' ", username);
            SqlCommand db = new SqlCommand(checkUserExistsQuery, connection);
            var countUser = (int)db.ExecuteScalar();

            // If they do exist, return
            if(countUser > 0)
            {
                connection.Close();
                return RedirectToAction("Register", "Home", "UserExists");
            }

            // If not, get a salt
            string getSalt = GetSalt();

            // Add it to the password
            var saltedPassword = password + getSalt;

            // Hash the password
            var hashedPassword = hashingin256(saltedPassword);

            // Insert the username, hashed password, salt
            string checkForUser = String.Format("INSERT INTO users (username, password, salt, firstname, lastname, email) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}') ", username, hashedPassword, getSalt, first, last, email);
            db = new SqlCommand(checkUserExistsQuery, connection);
            db.ExecuteNonQuery();

            connection.Close();

            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        public IActionResult SignIn()
        {
            return View(new SignInModel());
        }

        public IActionResult WithdrawMoney (string username, string accountFrom, int amount)
        {
            //Abdul start:


            //convert the amount into  pennies in order to store it into the database first

            amount *= 100;

            //withdraw cash query
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            string withdrawQuery = String.Format("insert into Transactions (account, amount, tranDesc) values ('{0}', '{1}', 'Withdrawal')", accountFrom, amount);
            SqlCommand db = new SqlCommand(withdrawQuery, connection);
            var withdraw = (int)db.ExecuteScalar();

            //make amount in dollars before displaying it
            amount /= 100;



            //sql statement to send this new accoutFrom and update the balance avaliable on that specific account


            Console.WriteLine($"Withdrawal of ${amount} was successful");

            connection.Close();



            return RedirectToAction("AccountActions", "Home", username);
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
            string transQuery1 = String.Format("insert into Transactions (account, amount, tranDesc) values ('{0}', '{1}', transfer from '{2}')", accountFrom, -amount, accountFrom);
            string transQuery2 = String.Format("insert into Transactions (account, amount, tranDesc) values ('{0}', '{1}', transfer to '{2}')", accountTo, amount, accountTo);
            SqlCommand db1 = new SqlCommand(transQuery1, connection);
            SqlCommand db2 = new SqlCommand(transQuery2, connection);

            var t1 = (int)db1.ExecuteScalar();
            var t2 = (int)db2.ExecuteScalar();

            //string sql;
            //sql = "insert into Transactions (account, amount, tranDesc) values (account number, amount, transfer from accout no)";
            //sql = "insert into Transactions (account, amount, tranDesc) values (account number, -amount, transfer from accout no)";

            Console.WriteLine("Transfered successfully");

            connection.Close();



            //Abdul's code ends here
            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult DepositMoney(string username, string account, double amount)
        {

            // Abdul:

            //convert the amount from dollars into pennies
            amount *= 100;

            //withdraw cash query
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            string depoQuery = String.Format("insert into Transactions (account, amount, tranDesc) values ('{0}', '{1}', 'deposit')", account, amount);
            SqlCommand db = new SqlCommand(depoQuery, connection);
            var deposit = (int)db.ExecuteScalar();


            Console.WriteLine("Transfered successfully");

            connection.Close();



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
