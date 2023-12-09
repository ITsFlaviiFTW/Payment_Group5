namespace Payment_Group5.Models
{
    public class CartModel
    {
        public List<string> products { get; set; }
        public string customerID { get; set; }
        public decimal total { get; set; }
    }
}
