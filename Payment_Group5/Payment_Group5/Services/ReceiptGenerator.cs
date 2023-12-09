using Microsoft.AspNetCore.Http;
using Payment_Group5.Models;
using PaymentModuleDemo.Models;
using System;
using System.Text;

namespace PaymentModuleDemo
{
    public interface IReceiptGenerator
    {
        string GenerateReceipt(dataBaseData data);
    }

    public class ReceiptGenerator : IReceiptGenerator
    {
        public string GenerateReceipt(dataBaseData data)
        {


                StringBuilder receiptBuilder = new StringBuilder();

                receiptBuilder.AppendLine("Payment Receipt");
                receiptBuilder.AppendLine("---------------");
                receiptBuilder.AppendLine($"Transaction ID: {data.orderId + data.date.Minute}");
                receiptBuilder.AppendLine($"CustomerId: {data.customerId}");

                receiptBuilder.AppendLine($"Subtotal: {data.subtotal:C}");
                receiptBuilder.AppendLine($"Tax: {data.total * 0.13m:C}");
                receiptBuilder.AppendLine($"Total: {data.total:C}");
                receiptBuilder.AppendLine($"Date: {data.date}");

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
