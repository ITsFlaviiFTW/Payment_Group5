using Payment_Group5.Models;

namespace PaymentModuleDemo.Models
{
    public class PaymentInfo
    {
        public List<string> Products { get; set; }
        public string CustomerID { get; set; }
        public decimal Total { get; set; } // This should be subtotal before tax and shipping
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public BillingAddressModel BillingAddress { get; set; }
        public ShippingModel Shipping { get; set; }
        public string PaymentMethod { get; set; } // e.g., "CreditCard"

        // Calculate shipping cost based on shipping option
        public decimal ShippingCost => Shipping?.ShippingOption == "Express" ? 9.99m : 0m;

        // Calculate final total
        public decimal FinalTotal => Total + ShippingCost; // Assume Tax is included in Total
    }
}
