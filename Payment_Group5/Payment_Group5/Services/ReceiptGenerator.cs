using Payment_Group5.Models;
using System;
using System.Text;

namespace PaymentModuleDemo
{
    public class ReceiptGenerator
    {
        public string GenerateReceipt(User user, Transaction transaction)
        {
            StringBuilder receiptBuilder = new StringBuilder();

            receiptBuilder.AppendLine("Payment Receipt");
            receiptBuilder.AppendLine("---------------");
            receiptBuilder.AppendLine($"Name: {user.Name}");
            receiptBuilder.AppendLine($"Email: {user.Email}");
            receiptBuilder.AppendLine($"Amount: {transaction.Amount:C}"); // ":C" formats as currency
            receiptBuilder.AppendLine($"Date: {transaction.TransactionDate}");

            return receiptBuilder.ToString();
        }
    }
}
