using System;
using System.Collections.Generic;

namespace PaymentModuleDemo
{
    public class Transaction
    {
        public int TransactionId { get; set; } // This will be auto-incremented by the database
        public List<string> ProductIds { get; set; } // Store Product IDs involved in the transaction
        public decimal Amount { get; set; } // Subtotal amount before taxes
        public decimal Tax { get; set; } // Calculated tax amount
        public decimal Total { get; set; } // Total amount after taxes
        public DateTime TransactionDate { get; set; } // Date of the transaction
        public string UserId { get; set; } // Foreign key to the User table
        // Constructor ensures the list is never null
        public Transaction()
        {
            ProductIds = new List<string>();
        }
    }
}
