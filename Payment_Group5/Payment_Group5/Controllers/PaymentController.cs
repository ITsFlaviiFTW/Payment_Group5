using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentModuleDemo;
using PaymentModuleDemo.Models;
using System;
using Payment_Group5.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Payment_Group5.Services;
using System.Data.SqlClient;

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
        [HttpPost("/api/cartinfo")]
        public IActionResult ReceiveCartData([FromBody] CartModel cart)
        {
            PaymentInfo paymentInfo = new PaymentInfo();
            paymentInfo.Total = cart.total;
            paymentInfo.Products = cart.products;
            paymentInfo.CustomerID = cart.customerID;


            double avg;
            double sum = 0;

            string connectionString = "Server=tcp:group5-payment-server.database.windows.net,1433;Initial Catalog=Group5-Payment-SQLDatabase;Persist Security Info=False;User ID=ktargosz;Password=paymentGr0up;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Pass the connection to the DatabaseControl class
                DatabaseControl.DatabaseConnection(connection);

                DatabaseControl.InsertOrder(connection, DatabaseControl.GetHighestOrderID(connection) + 1, paymentInfo.CustomerID, paymentInfo.Products.Count(), paymentInfo.Total, paymentInfo.Total, "Visa", DateTime.Now, paymentInfo.Total);

            }


            // Log the received data
            //_logger.LogInformation("Received cart data for CustomerID {CustomerID}", paymentInfo.CustomerID);

            // Store the received data in TempData for now 
            //TempData["Products"] = JsonConvert.SerializeObject(paymentInfo.Products);
            //TempData["CustomerID"] = paymentInfo.CustomerID;
            //TempData["Total"] = paymentInfo.Total;


            string checkoutUrl = Url.Action("BillingAddress", "Home", null, Request.Scheme);

            // Return a success response.
            return Ok(checkoutUrl );
        }

    }
}
