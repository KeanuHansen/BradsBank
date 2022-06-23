using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public List<List<string>> PartialTransactionHistory(string accountType)
        {
            var HistoryList = new List<List<string>>();

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

            return HistoryList;
        }

        public void OnGet()
        {
        }
    }
}
