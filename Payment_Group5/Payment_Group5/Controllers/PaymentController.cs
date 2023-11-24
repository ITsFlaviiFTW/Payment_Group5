using Microsoft.AspNetCore.Mvc;
using PaymentModuleDemo;
using PaymentModuleDemo.Models;
using System;

namespace Payment_Group5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IReceiptGenerator _receiptGenerator;

        public PaymentController(IReceiptGenerator receiptGenerator)
        {
            _receiptGenerator = receiptGenerator;
        }

        [HttpPost("ProcessPayment")]
        public IActionResult ProcessPayment([FromBody] PaymentInfo paymentInfo)
        {
            // Here we would implement the logic to process the payment

            // Simulate fetching user from the customerID in the paymentInfo
            User user = new User { UserId = paymentInfo.CustomerID, Name = "John Doe", Email = "john@example.com" };

            // Simulate transaction details
            Transaction transaction = new Transaction
            {
                UserId = paymentInfo.CustomerID, // Make sure this matches the type of UserId
                Amount = paymentInfo.Total, // This is the subtotal
                Tax = paymentInfo.Total * 0.1m, // Example tax calculation
                Total = paymentInfo.Total * 1.1m, // Total with tax
                TransactionDate = DateTime.Now,
                ProductIds = paymentInfo.Products // Assign the list of product IDs
            };

            // Generate receipt
            string receipt = _receiptGenerator.GenerateReceipt(user, transaction);

            // TODO: Save transaction details to the database
            // TODO: Send receipt to the user via email or other means

            return Ok(new { Receipt = receipt });
        }
    }
}
