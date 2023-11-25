using Microsoft.AspNetCore.Mvc;
using Payment_Group5.Models;
using System.Diagnostics;
using System;
using PaymentModuleDemo;
using Newtonsoft.Json;
using PaymentModuleDemo.Models;

namespace Payment_Group5.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult BillingAddress()
        {
            // Retrieve the cart data that was stored when the cart team's data was received
            var products = TempData["Products"];
            var customerID = TempData["CustomerID"];
            var total = TempData["Total"];

            // Reassign the TempData to persist for the next request
            TempData.Keep();

            // Pass the cart data to the view
            ViewData["Products"] = products;
            ViewData["CustomerID"] = customerID;
            ViewData["Total"] = total;

            return View(new BillingAddressModel());
        }

        [HttpPost]
        public IActionResult BillingAddress(BillingAddressModel model)
        {
            // Process the billing address and store it for use in the next step
            // For now, let's store it in TempData (or ideally in a session or database).
            TempData["BillingAddress"] = JsonConvert.SerializeObject(model);

            return RedirectToAction("Shipping");
        }
        public IActionResult Shipping()
        {
            // Retrieve the cart and billing data
            var billingAddress = TempData["BillingAddress"];
            var products = TempData["Products"];
            var customerID = TempData["CustomerID"];
            var total = TempData["Total"];

            // Reassign the TempData to persist for the next request
            TempData.Keep();

            // Pass this data to the view
            ViewData["BillingAddress"] = billingAddress;
            ViewData["Products"] = products;
            ViewData["CustomerID"] = customerID;
            ViewData["Total"] = total;

            return View(new ShippingModel());
        }

        [HttpPost]
        public IActionResult Shipping(ShippingModel model)
        {
            // Store the selected shipping option and continue the checkout process
            TempData["ShippingOption"] = model.ShippingOption;

            // Use the TempData to carry forward the cart and billing data
            TempData.Keep();

            return RedirectToAction("ProcessPayment");
        }


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
