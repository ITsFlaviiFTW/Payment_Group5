using Payment_Group5.Models;

namespace PaymentModuleDemo.Models
{
    public class PaymentInfo
    {
        public List<int> Products { get; set; }
        public int CustomerID { get; set; }
        public decimal Total { get; set; } // This should be subtotal before tax and shipping
        public BillingAddressModel BillingAddress { get; set; }
        public ShippingModel Shipping { get; set; }
        public string PaymentMethod { get; set; } // e.g., "CreditCard"

        // Calculate shipping cost based on shipping option
        public decimal ShippingCost => Shipping?.ShippingOption == "Express" ? 9.99m : 0m;

        // Calculate final total
        public decimal FinalTotal => Total + ShippingCost; // Assume Tax is included in Total
    }
}
