namespace Payment_Group5.Models
{
    public class dataBaseData
    {
        public int orderId { get; set; }
        public string customerId { get; set; }
        public int numOfItems { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
        public string paymentMethod { get; set; }
        public DateTime date { get; set; }
        public decimal avg { get; set; }
    }
}
