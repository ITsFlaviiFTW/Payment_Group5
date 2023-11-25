using System.Collections.Generic;

namespace PaymentModuleDemo.Models
{
    public class PaymentInfo
    {
        public List<int> Products { get; set; }
        public int CustomerID { get; set; }
        public decimal Total { get; set; }

        public static implicit operator PaymentInfo(PaymentInfo v)
        {
            throw new NotImplementedException();
        }
    }
}
