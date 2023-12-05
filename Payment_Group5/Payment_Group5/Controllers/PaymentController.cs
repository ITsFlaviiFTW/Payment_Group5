using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentModuleDemo;
using PaymentModuleDemo.Models;
using System;
using Payment_Group5.Models;
using System.Diagnostics;
using Newtonsoft.Json;



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

            // Store the received data in TempData for now 
            TempData["Products"] = JsonConvert.SerializeObject(paymentInfo.Products);
            TempData["CustomerID"] = paymentInfo.CustomerID;
            TempData["Total"] = paymentInfo.Total;


            string checkoutUrl = Url.Action("BillingAddress", "Home", null, Request.Scheme);

            // Return a success response.
            return Ok(new { message = "Cart data received successfully." });
        }

    }
}
