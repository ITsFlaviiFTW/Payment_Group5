using PaymentModuleDemo.Models;
using System;
using System.Text;

namespace PaymentModuleDemo
{
    public interface IReceiptGenerator
    {
        string GenerateReceipt(User user, Transaction transaction);
    }

    public class ReceiptGenerator : IReceiptGenerator
    {
        // Example service to fetch product details
        private readonly IProductService _productService;

        public ReceiptGenerator(IProductService productService)
        {
            _productService = productService;
        }

        public string GenerateReceipt(User user, Transaction transaction)
        {
            StringBuilder receiptBuilder = new StringBuilder();

            receiptBuilder.AppendLine("Payment Receipt");
            receiptBuilder.AppendLine("---------------");
            receiptBuilder.AppendLine($"Transaction ID: {transaction.TransactionId}");
            receiptBuilder.AppendLine($"Name: {user.Name}");
            receiptBuilder.AppendLine($"Email: {user.Email}");

            // Fetch product details using the product IDs and append them to the receipt
            foreach (var productId in transaction.ProductIds)
            {
                // Assuming GetProductById is a method that fetches product details
                var productDetail = _productService.GetProductById(productId);
                receiptBuilder.AppendLine($"Product: {productDetail.Name}, Price: {productDetail.Price:C}");
            }

            receiptBuilder.AppendLine($"Subtotal: {transaction.Amount:C}");
            receiptBuilder.AppendLine($"Tax: {transaction.Tax:C}");
            receiptBuilder.AppendLine($"Total: {transaction.Total:C}");
            receiptBuilder.AppendLine($"Date: {transaction.TransactionDate.ToString("g")}");

            return receiptBuilder.ToString();
        }
    }

    // Assuming you have a service like this
    public interface IProductService
    {
        ProductDetail GetProductById(int productId);
    }

    // ProductDetail class contains the details of a product
    public class ProductDetail
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        // Add other details as needed
    }
}
