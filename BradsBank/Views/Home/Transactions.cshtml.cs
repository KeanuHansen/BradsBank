using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;



namespace BradsBank.Views.Home
{
    public class TransactionsModel : PageModel
    {
        // Global Variable
        private string m_User;

        // Receive all the variables you pass in
        public TransactionsModel(string? username = "none")
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

        public List<List<string>> FullTransactionHistory
        {
            get
            {
                var HistoryList = new List<List<string>>();

                return HistoryList;
            }
        }

        public List<string> PartialTransactionHistory(string accountType)
        {
            //var HistoryList = new List<List<string>>();


            /*var i = 1;
            var number = new int[60];
            var items = new string[60];

            SqlCommand command = new SqlCommand(..., connection); ;
            var dr = command.ExecuteReader(); ;




            while (dr.Read())
            {
                number[i] = Convert.ToInt32(dr[0]);
                items[i] = Convert.ToString(dr[1]);
                ...

                              i = i + 1;
            };

            dr.Close();
            connection.Close();*/


            //private readonly ILogger<HomeController> _logger;
            // private readonly IConfiguration configuration;

            string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd" ;

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            var valuesList = new List<string>();


            connection.Open();
            //Read from the database

            string getTable = String.Format("SELECT COUNT(*) FROM account WHERE accountType = '{0}' ", accountType);
            SqlCommand command = new SqlCommand(getTable, connection);

            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                valuesList.Add((string)dataReader[0]);
            }
            connection.Close();

            // Get the transaction history data from the Database

            //item[0], item[1], item[2]

            // HOW IT SHOULD LOOK LIKE
            // <tr>
            // <th>Date</th>
            // <th>Type</th>
            // <th>Account</th>
            // <th>Amount</th>
            // </tr>

            // <tr>
            // <td>mm/dd/yyyy</td>
            // <td>Example</td>
            // <td>Account1</td>
            // <td>$0.00</td>
            // </tr>

            // HOW WE WILL RUN IT
            // <tr>
            // <td>item[0]</td>
            // <td>item[1]</td>
            // <td>item[2]</td>
            // <td>item[3]</td>
            // </tr>

            return valuesList;
        }

        public void OnGet()
        {
        }
    }
}
