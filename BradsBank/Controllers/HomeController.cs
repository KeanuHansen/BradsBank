using BradsBank.Models;
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
            return RedirectToAction("AccountActions", "Home", username);
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ValidateRegistration(string username, string password, string confirmed)
        {
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
