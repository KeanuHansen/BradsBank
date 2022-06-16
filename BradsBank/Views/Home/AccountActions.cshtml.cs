using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BradsBank.Views.Home
{
    public class AccountActionsModel : PageModel
    {
        // Global Variable
        private string m_User;

        // Receive all the variables you pass in
        public AccountActionsModel(string? username = "none")
        {
            // Assign them to global variables
            m_User = username;
        }

        public void OnGet()
        {
        }
    }
}
