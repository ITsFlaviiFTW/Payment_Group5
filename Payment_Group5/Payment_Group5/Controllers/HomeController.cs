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
        private readonly IReceiptGenerator _receiptGenerator;
        public HomeController(ILogger<HomeController> logger, IReceiptGenerator receiptGenerator)
        {
            _logger = logger;
            _receiptGenerator = receiptGenerator;
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
            Transaction transaction = new Transaction() { Amount = 250, TransactionDate = DateTime.Now };

            // Use the injected _receiptGenerator to generate the receipt
            string receipt = _receiptGenerator.GenerateReceipt(user, transaction);

            // Passing the receipt string to the view
            return View("Receipt", receipt);
        }
    }
}
