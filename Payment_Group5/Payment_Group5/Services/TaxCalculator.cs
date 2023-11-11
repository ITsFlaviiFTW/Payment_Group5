namespace Payment_Group5.Services
{
    public class TaxCalculator
    {
        public static double calculateGST(double subTotal)
        {
            return subTotal * 0.13;
        }
    }
}
