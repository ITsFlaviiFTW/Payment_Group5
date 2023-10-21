using Microsoft.AspNetCore.Mvc;
using Payment_Group5.Models;
using System.Diagnostics;
using System;
using PaymentModuleDemo;

namespace Payment_Group5.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GenerateReceipt()
        {
            // Sample data for the demonstration
            User user = new User() { Name = "John Doe", Email = "john@example.com" };
            Transaction transaction = new Transaction() { Amount = 250.75, TransactionDate = DateTime.Now };

            ReceiptGenerator receiptGenerator = new ReceiptGenerator();
            string receipt = receiptGenerator.GenerateReceipt(user, transaction);

            // Passing the receipt string to the view
            return View("Receipt", receipt);
        }
    }
}
