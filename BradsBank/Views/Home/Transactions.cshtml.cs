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
        private string m_Type;

        // Receive all the variables you pass in
        public TransactionsModel(string? username = "none", string? type = "none")
        {
            // Assign them to global variables
            m_User = username;
            m_Type = type;
        }

        public string Username
        {
            get
            {
                return m_User;
            }
        }

        public List<string> TransactionHistory
        {
            get
            {
                string connectionString = "Server=titan.cs.weber.edu, 10433;Database=AmandaShow;User ID=AmandaShow;Password=+his!$TheP@$$w0rd";

                SqlConnection connection = new SqlConnection(connectionString);

                var valuesList = new List<string>();

                var returnList = new List<List<string>>();

                connection.Open();
                //Read from the database

                string getTable = String.Format("SELECT Transactions.BUSINESSDATE, Account.ACCOUNTTYPE, Transactions.AMOUNT, Transactions.TRANSDESC FROM Transactions JOIN Account ON Account.Account = Transactions.Account WHERE Account.Username = '{0}'  ", m_User);

                if (m_Type == "none")
                {
                    getTable = String.Format("SELECT Transactions.BUSINESSDATE, Account.ACCOUNTTYPE, Transactions.AMOUNT, Transactions.TRANSDESC FROM Transactions JOIN Account ON Account.Account = Transactions.Account WHERE Account.Username = '{0}'  ", m_User);
                }
                else
                {
                    getTable = String.Format("SELECT Transactions.BUSINESSDATE, Account.ACCOUNTTYPE, Transactions.AMOUNT, Transactions.TRANSDESC FROM Transactions JOIN Account ON Account.Account = Transactions.Account WHERE Account.AccountType = '{0}' AND Account.Username = '{1}'  ", m_Type, m_User);
                }
                SqlCommand command = new SqlCommand(getTable, connection);

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var tempList = new List<string>();

                    // Date
                    var date = dataReader[0].ToString();

                    // Account Type
                    var type = dataReader[1].ToString();

                    // Amount
                    var amount = dataReader[2].ToString();

                    // Description
                    var description = dataReader[3].ToString();

                    // Add each to list
                    tempList.Add(date);
                    tempList.Add(type);
                    tempList.Add(amount);
                    tempList.Add(description);

                    // Add to list of lists
                    returnList.Add(tempList);

                    // Old code
                    valuesList.Add((string)dataReader[0]);
                }
                connection.Close();

                return valuesList;

            }
        }

        public void OnGet()
        {
        }
    }
}
