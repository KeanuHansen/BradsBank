using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BradsBank.Views.Home
{
    public class DepositModel : PageModel
    {
        // Global Variable
        private string m_User;

        // Receive all the variables you pass in
        public DepositModel(string? username = "none")
        {
            // Assign them to global variables
            m_User = username;
        }

        public string Username
        {
            get
            {
                return m_User;
            }
        }

        public string AmountSavings
        {
            get
            {
                SqlConnection connection = new SqlConnection("Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+h1sIsthenewP@ssword!");

                connection.Open();

                string query = String.Format("SELECT CURRENTBALANCE FROM ACCOUNT WHERE USERNAME = '{0}' AND ACCOUNTTYPE = 'Savings' ", m_User);
                SqlCommand db = new SqlCommand(query, connection);
                var amount = (Int64)db.ExecuteScalar();
                var amountDiv = double.Parse(amount.ToString()) / 100;
                string returnAmount = Math.Round(amountDiv, 2).ToString();
                connection.Close();

                return returnAmount.ToString();
            }
        }

        public string AmountCheckings
        {
            get
            {
                SqlConnection connection = new SqlConnection("Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd");

                connection.Open();

                string query = String.Format("SELECT CURRENTBALANCE FROM ACCOUNT WHERE USERNAME = '{0}' AND ACCOUNTTYPE = 'Checking' ", m_User);
                SqlCommand db = new SqlCommand(query, connection);
                var amount = (Int64)db.ExecuteScalar();
                var amountDiv = double.Parse(amount.ToString()) / 100;
                string returnAmount = Math.Round(amountDiv, 2).ToString();
                connection.Close();

                return returnAmount.ToString();
            }
        }

        public string AmountCredit
        {
            get
            {
                SqlConnection connection = new SqlConnection("Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd");

                connection.Open();

                string query = String.Format("SELECT CURRENTBALANCE FROM ACCOUNT WHERE USERNAME = '{0}' AND ACCOUNTTYPE = 'Credit Card' ", m_User);
                SqlCommand db = new SqlCommand(query, connection);
                var amount = (Int64)db.ExecuteScalar();
                var amountDiv = double.Parse(amount.ToString()) / 100;
                string returnAmount = Math.Round(amountDiv, 2).ToString();
                connection.Close();

                return returnAmount.ToString();
            }
        }

        public void OnGet()
        {
        }
    }
}
