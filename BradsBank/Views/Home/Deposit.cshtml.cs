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

        public string AmountOfMoney
        {
            get
            {
                SqlConnection connection = new SqlConnection("Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd");

                connection.Open();

                string query = String.Format("");
                SqlCommand db = new SqlCommand(query, connection);
                var amount = (Int16)db.ExecuteScalar();
                connection.Close();

                return "";
            }
        }

        public void OnGet()
        {
        }
    }
}
