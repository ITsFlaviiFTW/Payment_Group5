using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentModuleDemo;
using PaymentModuleDemo.Models;
using System;
using Payment_Group5.Models;
using System.Diagnostics;



namespace Payment_Group5.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IReceiptGenerator _receiptGenerator;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IReceiptGenerator receiptGenerator, ILogger<PaymentController> logger /*, DbContext context */)
        {
            _receiptGenerator = receiptGenerator;
            _logger = logger;
        }

        // This endpoint receives the cart data.
        [HttpPost("cartinfo")]
        public IActionResult ReceiveCartData([FromBody] PaymentInfo paymentInfo)
        {
            // Log the received data
            _logger.LogInformation("Received cart data for CustomerID {CustomerID}", paymentInfo.CustomerID);

            // Store the received data in session or a database.
            // For session:
            //HttpContext.Session.SetObject("PaymentInfo", paymentInfo);

            // For database (when integrating with kenneth's database):
            // var checkoutSession = new CheckoutSession
            // {
            //     CustomerId = paymentInfo.CustomerID,
            //     Products = JsonConvert.SerializeObject(paymentInfo.Products),
            //     TotalBeforeTax = paymentInfo.Total,
            //     // Set other properties as needed.
            // };
            // _context.CheckoutSessions.Add(checkoutSession);
            // _context.SaveChanges();

            // Return a success response.
            return Ok(new { message = "Cart data received successfully." });
        }

      /*  public IActionResult PaymentDetails()
        {
            // Retrieve all necessary data
            var billingAddress = TempData["BillingAddress"];
            var products = TempData["Products"];
            var customerID = TempData["CustomerID"];
            var total = TempData["Total"];
            var shippingOption = TempData["ShippingOption"];

            // Pass this data to the view
            ViewData["BillingAddress"] = billingAddress;
            ViewData["Products"] = products;
            ViewData["CustomerID"] = customerID;
            ViewData["Total"] = total;
            ViewData["ShippingOption"] = shippingOption;

            return View();
        }
        [HttpPost("ProcessPayment")]
        public IActionResult ProcessPayment(PaymentInfo paymentInfo)
        {
            // Assume we receive the name and email from the billing address form
            User user = new User
            {
                UserId = paymentInfo.CustomerID,
                Name = paymentInfo.BillingAddress.Name, // User-provided name
                Email = paymentInfo.BillingAddress.Email, // User-provided email
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
            string receipt = _receiptGenerator.GenerateReceipt(user, transaction);

            // TODO: Save transaction details to the database
            // TODO: Send receipt to the user via email or other means

            return View("Receipt", receipt);
        }*/

    }
}
