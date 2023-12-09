using Microsoft.AspNetCore.Mvc;
using Payment_Group5.Models;
using System.Diagnostics;
using System;
using PaymentModuleDemo;
using Newtonsoft.Json;
using PaymentModuleDemo.Models;
using Payment_Group5.Services;
using System.Data.SqlClient;
using System.Runtime.Intrinsics.X86;

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

        public IActionResult ProcessPayment()
        {
            // Retrieve and keep all necessary data
            var billingAddress = TempData["BillingAddress"];
            var products = TempData["Products"];
            var customerID = TempData["CustomerID"];
            var total = TempData["Total"];
            var shippingOption = TempData["ShippingOption"];

            TempData.Keep();

            // Pass this data to the view
            ViewData["BillingAddress"] = billingAddress;
            ViewData["Products"] = products;
            ViewData["CustomerID"] = customerID;
            ViewData["Total"] = total;
            ViewData["ShippingOption"] = shippingOption;

            return View(new PaymentInfo());
        }
        [HttpPost]
        public IActionResult ProcessPayment(PaymentInfo paymentInfo)
        {
            // Deserialize TempData to retrieve billing and shipping info
            var billingAddressJson = TempData["BillingAddress"] as string;
            var shippingOption = TempData["ShippingOption"] as string;

            if (!string.IsNullOrEmpty(billingAddressJson))
            {
                paymentInfo.BillingAddress = JsonConvert.DeserializeObject<BillingAddressModel>(billingAddressJson);
            }

            if (!string.IsNullOrEmpty(shippingOption))
            {
                paymentInfo.Shipping = new ShippingModel { ShippingOption = shippingOption };
            }

            // Verify ModelState is valid
            if (ModelState.IsValid)
            {
                // Handle invalid ModelState
                TempData.Keep();
                return View("ProcessPayment", paymentInfo);
            }

            // Proceed to generate the receipt
            User user = new User
            {
                UserId = paymentInfo.CustomerID,
                Name = paymentInfo.BillingAddress.Name,
                Email = paymentInfo.BillingAddress.Email,
                BillingAddress = $"{paymentInfo.BillingAddress.AddressLine1}, {paymentInfo.BillingAddress.City}",
            };

            // Calculate the final total, including any shipping costs and tax
            decimal finalTotal = paymentInfo.Total + paymentInfo.ShippingCost;
                decimal tax = finalTotal * 0.1m; // Example tax calculation (10%)
                finalTotal += tax;

                // Create a transaction instance
                Transaction transaction = new Transaction
                {
                    UserId = paymentInfo.CustomerID,
                    Amount = paymentInfo.Total, // This is the subtotal before shipping
                    Tax = tax, // Calculated tax amount
                    Total = finalTotal, // Total with shipping and tax
                    TransactionDate = DateTime.Now,
                    ProductIds = paymentInfo.Products // Assign the list of product IDs
                };

            // Generate receipt
            string connectionString = "Server=tcp:group5-payment-server.database.windows.net,1433;Initial Catalog=Group5-Payment-SQLDatabase;Persist Security Info=False;User ID=ktargosz;Password=paymentGr0up;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Pass the connection to the DatabaseControl class
                DatabaseControl.DatabaseConnection(connection);
                dataBaseData data = DatabaseControl.getLatestRecord(connection);
                string receipt = _receiptGenerator.GenerateReceipt(data);
                return View("Receipt", receipt);
            }

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

    }
}
