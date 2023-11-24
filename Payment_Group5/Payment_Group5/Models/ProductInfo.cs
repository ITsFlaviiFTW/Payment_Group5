using PaymentModuleDemo;

namespace Payment_Group5.Models
{
    public class ProductInfo
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Transaction Transaction { get; set; } // Navigation property for EF Core
        public int TransactionId { get; set; } // Foreign key to Transaction table
    }
}

