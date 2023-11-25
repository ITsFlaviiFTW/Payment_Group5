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
        public string GenerateReceipt(User user, Transaction transaction)
        {
            StringBuilder receiptBuilder = new StringBuilder();

                receiptBuilder.AppendLine("Payment Receipt");
                receiptBuilder.AppendLine("---------------");
                receiptBuilder.AppendLine($"Transaction ID: {transaction.TransactionId}");
                receiptBuilder.AppendLine($"Name: {user.Name}");
                receiptBuilder.AppendLine($"Email: {user.Email}");

                receiptBuilder.AppendLine($"Subtotal: {transaction.Amount:C}");
                receiptBuilder.AppendLine($"Tax: {transaction.Tax:C}");
                receiptBuilder.AppendLine($"Total: {transaction.Total:C}");
                receiptBuilder.AppendLine($"Date: {transaction.TransactionDate.ToString("g")}");

                return receiptBuilder.ToString();
            }
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
